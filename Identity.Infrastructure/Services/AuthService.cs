using Identity.Application.Contracts.Repositories;
using Identity.Application.Contracts.Services;
using Identity.Application.DTOs.Request;
using Identity.Application.DTOs.Response;
using Identity.Domain.Entities;                
using Identity.Infrastructure.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly INguoiDungRepository _userRepo;
        private readonly IVaiTroRepository _roleRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly JwtSettings _jwt;

        public AuthService(
            INguoiDungRepository userRepo,
            IVaiTroRepository roleRepo,
            IRefreshTokenRepository refreshRepo,
            IOptions<JwtSettings> jwtOptions)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _refreshRepo = refreshRepo;
            _jwt = jwtOptions.Value;
        }

        public int Register(RegisterRequest request)
        {
            var existing = _userRepo.GetByEmail(request.Email);
            if (existing != null)
                throw new InvalidOperationException("Email đã tồn tại.");

            var hashedPassword = PasswordHasher.Hash(request.Password);

            var user = new NguoiDung
            {
                HoTen = request.FullName,
                Email = request.Email,
                MatKhauHash = hashedPassword,
                VaiTroID = request.RoleId,
                TrangThai = true  
            };

            return _userRepo.Create(user);
        }

        public LoginResponse Login(LoginRequest request)
        {
            var user = _userRepo.GetByEmail(request.Email);
            if (user == null)
                throw new InvalidOperationException("Sai email hoặc mật khẩu.");

            if (!PasswordHasher.Verify(request.Password, user.MatKhauHash))
                throw new InvalidOperationException("Sai email hoặc mật khẩu.");

            if (!user.TrangThai)
                throw new InvalidOperationException("Tài khoản đã bị khóa.");

            var role = _roleRepo.GetById(user.VaiTroID);
            var roleCode = role?.MaVaiTro ?? "User";

            // 1. Tạo access token có jti
            var tokenResult = GenerateAccessToken(user, roleCode);

            // 2. Tạo refresh token
            var refreshTokenString = TokenGenerator.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.NguoiDungID,
                Token = refreshTokenString,
                JwtId = tokenResult.JwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays),
                IsRevoked = false,
                IsUsed = false
            };
            _refreshRepo.Create(refreshTokenEntity);

            return new LoginResponse
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = refreshTokenString,
                ExpiresAt = tokenResult.ExpiresAt,
                FullName = user.HoTen,
                Email = user.Email,
                Role = roleCode
            };
        }

        public LoginResponse Refresh(RefreshTokenRequest request)
        {
            var storedToken = _refreshRepo.GetByToken(request.RefreshToken);
            if (storedToken == null)
                throw new InvalidOperationException("Refresh token không hợp lệ.");

            if (storedToken.IsRevoked)
                throw new InvalidOperationException("Refresh token đã bị thu hồi.");

            if (storedToken.IsUsed)
                throw new InvalidOperationException("Refresh token đã được sử dụng.");

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                throw new InvalidOperationException("Refresh token đã hết hạn.");

            var user = _userRepo.GetById(storedToken.UserId);
            if (user == null)
                throw new InvalidOperationException("Người dùng không tồn tại.");

            var role = _roleRepo.GetById(user.VaiTroID);
            var roleCode = role?.MaVaiTro ?? "User";

            _refreshRepo.MarkUsed(storedToken.RefreshTokenId);

            // 2. Sinh access token mới
            var tokenResult = GenerateAccessToken(user, roleCode);

            // 3. Tạo refresh token mới
            var newRefreshTokenString = TokenGenerator.GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.NguoiDungID,
                Token = newRefreshTokenString,
                JwtId = tokenResult.JwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays),
                IsRevoked = false,
                IsUsed = false
            };
            _refreshRepo.Create(newRefreshTokenEntity);

            return new LoginResponse
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = newRefreshTokenString,
                ExpiresAt = tokenResult.ExpiresAt,
                FullName = user.HoTen,
                Email = user.Email,
                Role = roleCode
            };
        }

        public void RevokeRefreshToken(RefreshTokenRequest request)
        {
            var storedToken = _refreshRepo.GetByToken(request.RefreshToken);
            if (storedToken == null)
                throw new InvalidOperationException("Refresh token không tồn tại.");

            if (storedToken.IsRevoked)
                return;

            _refreshRepo.Revoke(storedToken.RefreshTokenId);
        }

        private (string AccessToken, DateTime ExpiresAt, string JwtId)
            GenerateAccessToken(NguoiDung user, string role)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwt.Key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jti = Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.NguoiDungID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   jti),
                new Claim("fullName",                    user.HoTen),
                new Claim(ClaimTypes.Role,               role)
            };

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_jwt.ExpiresMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (accessToken, expires, jti);
        }
        public void RevokeAllRefreshTokens(RefreshTokenRequest request)
        {
            var storedToken = _refreshRepo.GetByToken(request.RefreshToken);
            if (storedToken == null)
                throw new InvalidOperationException("Refresh token không tồn tại.");

            _refreshRepo.RevokeAllByUser(storedToken.UserId);
        }

    }
}

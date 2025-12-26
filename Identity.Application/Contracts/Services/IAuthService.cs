using Identity.Application.DTOs.Request;
using Identity.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Services
{
    public interface IAuthService
    {
        int Register(RegisterRequest request);
        LoginResponse Login(LoginRequest request);
        LoginResponse Refresh(RefreshTokenRequest request);
        void RevokeRefreshToken(RefreshTokenRequest request);
        void RevokeAllRefreshTokens(RefreshTokenRequest request);

    }
}

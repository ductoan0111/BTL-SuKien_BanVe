using BuildingBlocks.Shareds.Abstractions;
using Identity.Application.Contracts.Repositories;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RefreshTokenRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public bool RevokeAllByUser(int userId)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
    UPDATE RefreshToken
    SET IsRevoked = 1,
        RevokedAt = GETDATE()
    WHERE UserId = @UserId
      AND IsRevoked = 0;";

            AddParam(cmd, "@UserId", userId);
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }


        public int Create(RefreshToken token)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO RefreshToken (UserId, Token, JwtId, ExpiresAt, CreatedAt, IsRevoked, IsUsed)
            OUTPUT INSERTED.RefreshTokenId
            VALUES (@UserId, @Token, @JwtId, @ExpiresAt, @CreatedAt, @IsRevoked, @IsUsed);";

            AddParam(cmd, "@UserId", token.UserId);
            AddParam(cmd, "@Token", token.Token);
            AddParam(cmd, "@JwtId", token.JwtId);
            AddParam(cmd, "@ExpiresAt", token.ExpiresAt);
            AddParam(cmd, "@CreatedAt", token.CreatedAt);
            AddParam(cmd, "@IsRevoked", token.IsRevoked);
            AddParam(cmd, "@IsUsed", token.IsUsed);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public RefreshToken? GetByToken(string token)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT RefreshTokenId, UserId, Token, JwtId, ExpiresAt, CreatedAt,
                   RevokedAt, IsRevoked, IsUsed
            FROM RefreshToken
            WHERE Token = @Token";

            AddParam(cmd, "@Token", token);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public bool MarkUsed(int refreshTokenId)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE RefreshToken
            SET IsUsed = 1
            WHERE RefreshTokenId = @Id";

            AddParam(cmd, "@Id", refreshTokenId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Revoke(int refreshTokenId)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE RefreshToken
            SET IsRevoked = 1,
                RevokedAt = GETDATE()
            WHERE RefreshTokenId = @Id";

            AddParam(cmd, "@Id", refreshTokenId);
            return cmd.ExecuteNonQuery() > 0;
        }

        private static void AddParam(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static RefreshToken Map(IDataRecord r)
        {
            var t = new RefreshToken
            {
                RefreshTokenId = r.GetInt32(r.GetOrdinal("RefreshTokenId")),
                UserId = r.GetInt32(r.GetOrdinal("UserId")),
                Token = r.GetString(r.GetOrdinal("Token")),
                JwtId = r.GetString(r.GetOrdinal("JwtId")),
                ExpiresAt = r.GetDateTime(r.GetOrdinal("ExpiresAt")),
                CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt")),
                IsRevoked = r.GetBoolean(r.GetOrdinal("IsRevoked")),
                IsUsed = r.GetBoolean(r.GetOrdinal("IsUsed"))
            };

            int idxRevokedAt = r.GetOrdinal("RevokedAt");
            if (!r.IsDBNull(idxRevokedAt))
                t.RevokedAt = r.GetDateTime(idxRevokedAt);

            return t;
        }
    }
}

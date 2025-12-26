using System.Data;
using BuildingBlocks.Shareds.Abstractions;
using Identity.Application.Contracts.Repositories;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public NguoiDungRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<NguoiDung> GetAll()
        {
            var list = new List<NguoiDung>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT NguoiDungID, HoTen, Email, MatKhauHash, VaiTroID, NgayTao, TrangThai
FROM NguoiDung";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public NguoiDung? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT NguoiDungID, HoTen, Email, MatKhauHash, VaiTroID, NgayTao, TrangThai
FROM NguoiDung
WHERE NguoiDungID = @Id";

            AddParam(cmd, "@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public NguoiDung? GetByEmail(string email)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT NguoiDungID, HoTen, Email, MatKhauHash, VaiTroID, NgayTao, TrangThai
            FROM NguoiDung
            WHERE Email = @Email";

            AddParam(cmd, "@Email", email);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public int Create(NguoiDung user)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            // NgayTao dùng DEFAULT GETDATE(), không cần truyền
            cmd.CommandText = @"
            INSERT INTO NguoiDung (HoTen, Email, MatKhauHash, VaiTroID, TrangThai)
            OUTPUT INSERTED.NguoiDungID
            VALUES (@HoTen, @Email, @MatKhauHash, @VaiTroID, @TrangThai);";

            AddParam(cmd, "@HoTen", user.HoTen);
            AddParam(cmd, "@Email", user.Email);
            AddParam(cmd, "@MatKhauHash", user.MatKhauHash);
            AddParam(cmd, "@VaiTroID", user.VaiTroID);
            AddParam(cmd, "@TrangThai", user.TrangThai);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public bool Update(NguoiDung user)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE NguoiDung
            SET HoTen       = @HoTen,
                Email       = @Email,
                MatKhauHash = @MatKhauHash,
                VaiTroID    = @VaiTroID,
                TrangThai   = @TrangThai
            WHERE NguoiDungID = @Id";

            AddParam(cmd, "@HoTen", user.HoTen);
            AddParam(cmd, "@Email", user.Email);
            AddParam(cmd, "@MatKhauHash", user.MatKhauHash);
            AddParam(cmd, "@VaiTroID", user.VaiTroID);
            AddParam(cmd, "@TrangThai", user.TrangThai);
            AddParam(cmd, "@Id", user.NguoiDungID);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE NguoiDung
            SET TrangThai = 0
            WHERE NguoiDungID = @Id";

            AddParam(cmd, "@Id", id);

            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        private static void AddParam(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static NguoiDung Map(IDataRecord r)
        {
            return new NguoiDung
            {
                NguoiDungID = r.GetInt32(r.GetOrdinal("NguoiDungID")),
                HoTen = r.GetString(r.GetOrdinal("HoTen")),
                Email = r.GetString(r.GetOrdinal("Email")),
                MatKhauHash = r.GetString(r.GetOrdinal("MatKhauHash")),
                VaiTroID = r.GetInt32(r.GetOrdinal("VaiTroID")),
                NgayTao = r.GetDateTime(r.GetOrdinal("NgayTao")),
                TrangThai = r.GetBoolean(r.GetOrdinal("TrangThai"))
            };
        }
    }
}

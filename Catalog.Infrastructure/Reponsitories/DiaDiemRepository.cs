using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using System.Data;
using BuildingBlocks.Shareds.Abstractions;

namespace Catalog.Infrastructure.Reponsitories
{
    public class DiaDiemRepository : IDiaDiemReponsitory
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DiaDiemRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<DiaDiem> GetAll(byte? trangThai = null)
        {
            var list = new List<DiaDiem>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT DiaDiemId, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
FROM dbo.DiaDiem
WHERE (@TrangThai IS NULL OR TrangThai = @TrangThai)
ORDER BY DiaDiemId DESC;";

            AddParam(cmd, "@TrangThai", trangThai.HasValue ? (object)trangThai.Value : DBNull.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public DiaDiem? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT DiaDiemId, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
FROM dbo.DiaDiem
WHERE DiaDiemId = @Id;";

            AddParam(cmd, "@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public int Create(DiaDiem entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.DiaDiem (TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai)
OUTPUT INSERTED.DiaDiemId
VALUES (@Ten, @DiaChi, @SucChua, @MoTa, @TrangThai);";

            AddParam(cmd, "@Ten", entity.TenDiaDiem);
            AddParam(cmd, "@DiaChi", entity.DiaChi);
            AddParam(cmd, "@SucChua", (object?)entity.SucChua ?? DBNull.Value);
            AddParam(cmd, "@MoTa", (object?)entity.MoTa ?? DBNull.Value);

            // nếu không set thì mặc định 1
            var trangThai = entity.TrangThai;
            if (trangThai != 0 && trangThai != 1 && trangThai != 2) trangThai = 1;
            AddParam(cmd, "@TrangThai", trangThai);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public bool Update(DiaDiem entity)
        {
            // validate trạng thái 0/1/2
            if (entity.TrangThai != 0 && entity.TrangThai != 1 && entity.TrangThai != 2)
                entity.TrangThai = 1;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE dbo.DiaDiem
            SET TenDiaDiem = @Ten,
                DiaChi     = @DiaChi,
                SucChua    = @SucChua,
                MoTa       = @MoTa,
                TrangThai  = @TrangThai
            WHERE DiaDiemId = @Id;";

            AddParam(cmd, "@Ten", entity.TenDiaDiem);
            AddParam(cmd, "@DiaChi", entity.DiaChi);
            AddParam(cmd, "@SucChua", (object?)entity.SucChua ?? DBNull.Value);
            AddParam(cmd, "@MoTa", (object?)entity.MoTa ?? DBNull.Value);
            AddParam(cmd, "@TrangThai", entity.TrangThai);
            AddParam(cmd, "@Id", entity.DiaDiemId);

            return cmd.ExecuteNonQuery() > 0;
        }


        public bool UpdateTrangThai(int id, byte trangThai)
        {
            if (trangThai != 0 && trangThai != 1 && trangThai != 2)
                return false;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE dbo.DiaDiem
            SET TrangThai = @TrangThai
            WHERE DiaDiemId = @Id;";

            AddParam(cmd, "@Id", id);
            AddParam(cmd, "@TrangThai", trangThai);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.DiaDiem
SET TrangThai = 0
WHERE DiaDiemId = @Id;";

            AddParam(cmd, "@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private static void AddParam(IDbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static DiaDiem Map(IDataRecord r)
        {
            return new DiaDiem
            {
                DiaDiemId = Convert.ToInt32(r["DiaDiemId"]),
                TenDiaDiem = Convert.ToString(r["TenDiaDiem"])!,
                DiaChi = Convert.ToString(r["DiaChi"])!,
                SucChua = r["SucChua"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["SucChua"]),
                MoTa = r["MoTa"] == DBNull.Value ? null : Convert.ToString(r["MoTa"]),
                TrangThai = r["TrangThai"] == DBNull.Value ? (byte)1 : Convert.ToByte(r["TrangThai"])
            };
        }
    }
}

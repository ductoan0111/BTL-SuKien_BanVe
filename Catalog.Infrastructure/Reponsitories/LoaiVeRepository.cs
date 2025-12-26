using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using System.Data;
using BuildingBlocks.Shareds.Abstractions;

namespace Catalog.Infrastructure.Reponsitories
{
    public class LoaiVeRepository : ILoaiVeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LoaiVeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<LoaiVe> GetAll()
        {
            var list = new List<LoaiVe>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT LoaiVeId, SuKienId, TenLoaiVe, DonGia,
       SoLuongToiDa, GioiHanMoiKhach, TrangThai
FROM LoaiVe
WHERE TrangThai = 1
ORDER BY LoaiVeId DESC;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public List<LoaiVe> GetBySuKien(int suKienId)
        {
            var list = new List<LoaiVe>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT LoaiVeId, SuKienId, TenLoaiVe, DonGia,
       SoLuongToiDa, GioiHanMoiKhach, TrangThai
FROM LoaiVe
WHERE SuKienId = @SuKienId AND TrangThai = 1
ORDER BY LoaiVeId DESC;";

            AddParam(cmd, "@SuKienId", suKienId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public LoaiVe? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT LoaiVeId, SuKienId, TenLoaiVe, DonGia,
       SoLuongToiDa, GioiHanMoiKhach, TrangThai
FROM LoaiVe
WHERE LoaiVeId = @Id AND TrangThai = 1;";

            AddParam(cmd, "@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public int Create(LoaiVe entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO LoaiVe
    (SuKienId, TenLoaiVe, DonGia, SoLuongToiDa, GioiHanMoiKhach, TrangThai)
OUTPUT INSERTED.LoaiVeId
VALUES
    (@SuKienId, @Ten, @DonGia, @SoLuongToiDa, @GioiHan, 1);";

            AddParam(cmd, "@SuKienId", entity.SuKienId);
            AddParam(cmd, "@Ten", entity.TenLoaiVe);
            AddParam(cmd, "@DonGia", entity.DonGia);
            AddParam(cmd, "@SoLuongToiDa", entity.SoLuongToiDa);
            AddParam(cmd, "@GioiHan", entity.GioiHanMoiKhach);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public bool Update(LoaiVe entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
UPDATE LoaiVe
SET SuKienId        = @SuKienId,
    TenLoaiVe       = @Ten,
    DonGia          = @DonGia,
    SoLuongToiDa    = @SoLuongToiDa,
    GioiHanMoiKhach = @GioiHan
WHERE LoaiVeId = @Id AND TrangThai = 1;";

            AddParam(cmd, "@SuKienId", entity.SuKienId);
            AddParam(cmd, "@Ten", entity.TenLoaiVe);
            AddParam(cmd, "@DonGia", entity.DonGia);
            AddParam(cmd, "@SoLuongToiDa", entity.SoLuongToiDa);
            AddParam(cmd, "@GioiHan", entity.GioiHanMoiKhach);
            AddParam(cmd, "@Id", entity.LoaiVeId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // SOFT DELETE: TrangThai = 0
        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
UPDATE LoaiVe
SET TrangThai = 0
WHERE LoaiVeId = @Id AND TrangThai = 1;";

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

        private static LoaiVe Map(IDataRecord r)
        {
            return new LoaiVe
            {
                LoaiVeId = Convert.ToInt32(r["LoaiVeId"]),
                SuKienId = Convert.ToInt32(r["SuKienId"]),
                TenLoaiVe = Convert.ToString(r["TenLoaiVe"])!,
                DonGia = Convert.ToDecimal(r["DonGia"]),
                SoLuongToiDa = Convert.ToInt32(r["SoLuongToiDa"]),
                GioiHanMoiKhach = r["GioiHanMoiKhach"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(r["GioiHanMoiKhach"]),
                TrangThai = Convert.ToBoolean(r["TrangThai"])
            };
        }
    }
}

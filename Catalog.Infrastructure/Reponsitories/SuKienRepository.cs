using Catalog.Application.Contracts.Interface;
using Catalog.Domain.Entities;
using System.Data;
using BuildingBlocks.Shareds.Abstractions;

namespace Catalog.Infrastructure.Reponsitories
{
    public class SuKienRepository : ISuKienRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SuKienRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public List<SuKien> GetAll(bool? trangThai = true)
        {
            var list = new List<SuKien>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT SuKienId, TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc,
                       DiaDiemId, ToChucId, TrangThai
                FROM dbo.SuKien
                WHERE (@TrangThai IS NULL OR TrangThai = @TrangThai)
                ORDER BY SuKienId DESC;";

            AddParam(cmd, "@TrangThai", trangThai.HasValue ? (object)trangThai.Value : DBNull.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public SuKien? GetById(int id, bool? trangThai = true)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT SuKienId, TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc,
                   DiaDiemId, ToChucId, TrangThai
            FROM dbo.SuKien
            WHERE SuKienId = @Id
              AND (@TrangThai IS NULL OR TrangThai = @TrangThai);";

            AddParam(cmd, "@Id", id);
            AddParam(cmd, "@TrangThai", trangThai.HasValue ? (object)trangThai.Value : DBNull.Value);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        public List<SuKien> GetByDiaDiem(int diaDiemId, bool? trangThai = true)
        {
            var list = new List<SuKien>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT SuKienId, TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc,
                   DiaDiemId, ToChucId, TrangThai
            FROM dbo.SuKien
            WHERE DiaDiemId = @DiaDiemId
              AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
            ORDER BY SuKienId DESC;";

            AddParam(cmd, "@DiaDiemId", diaDiemId);
            AddParam(cmd, "@TrangThai", trangThai.HasValue ? (object)trangThai.Value : DBNull.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public int Create(SuKien entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO dbo.SuKien
                (TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc, DiaDiemId, ToChucId, TrangThai)
            OUTPUT INSERTED.SuKienId
            VALUES
                (@Ten, @MoTa, @BatDau, @KetThuc, @DiaDiemId, @ToChucId, @TrangThai);";

            AddParam(cmd, "@Ten", entity.TenSuKien);
            AddParam(cmd, "@MoTa", (object?)entity.MoTa ?? DBNull.Value);
            AddParam(cmd, "@BatDau", entity.ThoiGianBatDau);
            AddParam(cmd, "@KetThuc", entity.ThoiGianKetThuc);
            AddParam(cmd, "@DiaDiemId", entity.DiaDiemId);
            AddParam(cmd, "@ToChucId", entity.ToChucId);

            // nếu không set thì mặc định mở/hoạt động = true
            AddParam(cmd, "@TrangThai", entity.TrangThai);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public bool Update(SuKien entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE dbo.SuKien
            SET TenSuKien       = @Ten,
                MoTa            = @MoTa,
                ThoiGianBatDau  = @BatDau,
                ThoiGianKetThuc = @KetThuc,
                DiaDiemId       = @DiaDiemId,
                ToChucId        = @ToChucId,
                TrangThai       = @TrangThai
            WHERE SuKienId = @Id;";

            AddParam(cmd, "@Ten", entity.TenSuKien);
            AddParam(cmd, "@MoTa", (object?)entity.MoTa ?? DBNull.Value);
            AddParam(cmd, "@BatDau", entity.ThoiGianBatDau);
            AddParam(cmd, "@KetThuc", entity.ThoiGianKetThuc);
            AddParam(cmd, "@DiaDiemId", entity.DiaDiemId);
            AddParam(cmd, "@ToChucId", entity.ToChucId);
            AddParam(cmd, "@TrangThai", entity.TrangThai);
            AddParam(cmd, "@Id", entity.SuKienId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // SOFT DELETE: TrangThai = 0
        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE dbo.SuKien
            SET TrangThai = 0
            WHERE SuKienId = @Id;";

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

        private static SuKien Map(IDataRecord r)
        {
            return new SuKien
            {
                SuKienId = Convert.ToInt32(r["SuKienId"]),
                TenSuKien = Convert.ToString(r["TenSuKien"])!,
                MoTa = r["MoTa"] == DBNull.Value ? null : Convert.ToString(r["MoTa"]),
                ThoiGianBatDau = Convert.ToDateTime(r["ThoiGianBatDau"]),
                ThoiGianKetThuc = Convert.ToDateTime(r["ThoiGianKetThuc"]),
                DiaDiemId = Convert.ToInt32(r["DiaDiemId"]),
                ToChucId = Convert.ToInt32(r["ToChucId"]),
                TrangThai = Convert.ToBoolean(r["TrangThai"])
            };
        }
    }
}

using BuildingBlocks.Shareds.Abstractions;
using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infractructure.Reponsitories
{
    public class DiaDiemReponsitory : IDiaDiemReponsitory
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DiaDiemReponsitory(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Create(DiaDiem entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.DiaDiem (TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai)
OUTPUT INSERTED.DiaDiemID
VALUES (@TenDiaDiem, @DiaChi, @SucChua, @MoTa, @TrangThai);";

            AddParam(cmd, "@TenDiaDiem", entity.TenDiaDiem);
            AddParam(cmd, "@DiaChi", entity.DiaChi);
            AddParam(cmd, "@SucChua", entity.SucChua);
            AddParam(cmd, "@MoTa", entity.MoTa);
            AddParam(cmd, "@TrangThai", entity.TrangThai);

            var obj = cmd.ExecuteScalar();
            return Convert.ToInt32(obj);
        }

        public bool Update(DiaDiem entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.DiaDiem
SET TenDiaDiem = @TenDiaDiem,
    DiaChi     = @DiaChi,
    SucChua    = @SucChua,
    MoTa       = @MoTa,
    TrangThai  = @TrangThai
WHERE DiaDiemID = @Id;";

            AddParam(cmd, "@Id", entity.DiaDiemID);
            AddParam(cmd, "@TenDiaDiem", entity.TenDiaDiem);
            AddParam(cmd, "@DiaChi", entity.DiaChi);
            AddParam(cmd, "@SucChua", entity.SucChua);
            AddParam(cmd, "@MoTa", entity.MoTa);
            AddParam(cmd, "@TrangThai", entity.TrangThai);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE dbo.DiaDiem SET TrangThai = 0 WHERE DiaDiemID = @Id;";
            AddParam(cmd, "@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public List<DiaDiem> GetAll()
        {
            var list = new List<DiaDiem>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT DiaDiemID, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
FROM dbo.DiaDiem
ORDER BY DiaDiemID DESC;";

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
SELECT DiaDiemID, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
FROM dbo.DiaDiem
WHERE DiaDiemID = @Id;";

            AddParam(cmd, "@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
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
                DiaDiemID = r.GetInt32(r.GetOrdinal("DiaDiemID")),
                TenDiaDiem = r.IsDBNull(r.GetOrdinal("TenDiaDiem")) ? null : r.GetString(r.GetOrdinal("TenDiaDiem")),
                DiaChi = r.GetString(r.GetOrdinal("DiaChi")),
                SucChua = r.IsDBNull(r.GetOrdinal("SucChua")) ? (int?)null : r.GetInt32(r.GetOrdinal("SucChua")),
                MoTa = r.IsDBNull(r.GetOrdinal("MoTa")) ? null : r.GetString(r.GetOrdinal("MoTa")),
                TrangThai = r.GetBoolean(r.GetOrdinal("TrangThai"))
            };
        }
    }
}

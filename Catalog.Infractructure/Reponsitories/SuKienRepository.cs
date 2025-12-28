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
    public class SuKienRepository : ISuKienRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SuKienRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<SuKien> GetAll(bool onlyActive = false)
        {
            var list = new List<SuKien>();
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT * FROM SuKien
            WHERE (@OnlyActive = 0 OR TrangThai = 1)
            ORDER BY ThoiGianBatDau DESC";

            AddParam(cmd, "@OnlyActive", onlyActive ? 1 : 0);

            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public SuKien? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SuKien WHERE SuKienID = @Id";
            AddParam(cmd, "@Id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Create(SuKien e)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        INSERT INTO SuKien
        (DanhMucID, DiaDiemID, ToChucID, TenSuKien, MoTa,
         ThoiGianBatDau, ThoiGianKetThuc, AnhBiaUrl, TrangThai, NgayTao)
        OUTPUT INSERTED.SuKienID
        VALUES
        (@DanhMuc, @DiaDiem, @ToChuc, @Ten, @MoTa,
         @BatDau, @KetThuc, @AnhBia, @TrangThai, GETDATE())";

            AddParam(cmd, "@DanhMuc", e.DanhMucID);
            AddParam(cmd, "@DiaDiem", e.DiaDiemID);
            AddParam(cmd, "@ToChuc", e.ToChucID);
            AddParam(cmd, "@Ten", e.TenSuKien);
            AddParam(cmd, "@MoTa", e.MoTa);
            AddParam(cmd, "@BatDau", e.ThoiGianBatDau);
            AddParam(cmd, "@KetThuc", e.ThoiGianKetThuc);
            AddParam(cmd, "@AnhBia", e.AnhBiaUrl);
            AddParam(cmd, "@TrangThai", e.TrangThai);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(SuKien e)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE SuKien SET
            DanhMucID=@DanhMuc,
            DiaDiemID=@DiaDiem,
            ToChucID=@ToChuc,
            TenSuKien=@Ten,
            MoTa=@MoTa,
            ThoiGianBatDau=@BatDau,
            ThoiGianKetThuc=@KetThuc,
            AnhBiaUrl=@AnhBia,
            TrangThai=@TrangThai
            WHERE SuKienID=@Id";

            AddParam(cmd, "@Id", e.SuKienID);
            AddParam(cmd, "@DanhMuc", e.DanhMucID);
            AddParam(cmd, "@DiaDiem", e.DiaDiemID);
            AddParam(cmd, "@ToChuc", e.ToChucID);
            AddParam(cmd, "@Ten", e.TenSuKien);
            AddParam(cmd, "@MoTa", e.MoTa);
            AddParam(cmd, "@BatDau", e.ThoiGianBatDau);
            AddParam(cmd, "@KetThuc", e.ThoiGianKetThuc);
            AddParam(cmd, "@AnhBia", e.AnhBiaUrl);
            AddParam(cmd, "@TrangThai", e.TrangThai);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SuKien SET TrangThai = 0 WHERE SuKienID = @Id";
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

        private static SuKien Map(IDataRecord r) => new()
        {
            SuKienID = (int)r["SuKienID"],
            DanhMucID = (int)r["DanhMucID"],
            DiaDiemID = (int)r["DiaDiemID"],
            ToChucID = (int)r["ToChucID"],
            TenSuKien = r["TenSuKien"].ToString()!,
            MoTa = r["MoTa"] as string,
            ThoiGianBatDau = (DateTime)r["ThoiGianBatDau"],
            ThoiGianKetThuc = (DateTime)r["ThoiGianKetThuc"],
            AnhBiaUrl = r["AnhBiaUrl"] as string,
            TrangThai = (byte)r["TrangThai"],
            NgayTao = (DateTime)r["NgayTao"]
        };
    }
}

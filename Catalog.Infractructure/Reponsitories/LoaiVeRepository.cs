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
    public class LoaiVeRepository : ILoaiVeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public LoaiVeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<LoaiVe> GetBySuKien(int suKienId, bool onlyActive = false)
        {
            var list = new List<LoaiVe>();
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT * FROM LoaiVe
            WHERE SuKienID = @SuKienID
              AND (@OnlyActive = 0 OR TrangThai = 1)
            ORDER BY DonGia ASC";

            Add(cmd, "@SuKienID", suKienId);
            Add(cmd, "@OnlyActive", onlyActive ? 1 : 0);

            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public LoaiVe? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM LoaiVe WHERE LoaiVeID = @Id";
            Add(cmd, "@Id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public int Create(LoaiVe e)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO LoaiVe
            (SuKienID, TenLoaiVe, MoTa, DonGia, SoLuongToiDa, SoLuongDaBan,
             GioiHanMoiKhach, ThoiGianMoBan, ThoiGianDongBan, TrangThai)
            OUTPUT INSERTED.LoaiVeID
            VALUES
            (@SuKienID, @Ten, @MoTa, @Gia, @ToiDa, 0,
             @GioiHan, @MoBan, @DongBan, @TrangThai)";

            Add(cmd, "@SuKienID", e.SuKienID);
            Add(cmd, "@Ten", e.TenLoaiVe);
            Add(cmd, "@MoTa", e.MoTa);
            Add(cmd, "@Gia", e.DonGia);
            Add(cmd, "@ToiDa", e.SoLuongToiDa);
            Add(cmd, "@GioiHan", e.GioiHanMoiKhach);
            Add(cmd, "@MoBan", e.ThoiGianMoBan);
            Add(cmd, "@DongBan", e.ThoiGianDongBan);
            Add(cmd, "@TrangThai", e.TrangThai);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(LoaiVe e)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE LoaiVe SET
            TenLoaiVe=@Ten,
            MoTa=@MoTa,
            DonGia=@Gia,
            SoLuongToiDa=@ToiDa,
            GioiHanMoiKhach=@GioiHan,
            ThoiGianMoBan=@MoBan,
            ThoiGianDongBan=@DongBan,
            TrangThai=@TrangThai
            WHERE LoaiVeID=@Id";

            Add(cmd, "@Id", e.LoaiVeID);
            Add(cmd, "@Ten", e.TenLoaiVe);
            Add(cmd, "@MoTa", e.MoTa);
            Add(cmd, "@Gia", e.DonGia);
            Add(cmd, "@ToiDa", e.SoLuongToiDa);
            Add(cmd, "@GioiHan", e.GioiHanMoiKhach);
            Add(cmd, "@MoBan", e.ThoiGianMoBan);
            Add(cmd, "@DongBan", e.ThoiGianDongBan);
            Add(cmd, "@TrangThai", e.TrangThai);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE LoaiVe SET TrangThai = 0 WHERE LoaiVeID = @Id";
            Add(cmd, "@Id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private static void Add(IDbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static LoaiVe Map(IDataRecord r) => new()
        {
            LoaiVeID = (int)r["LoaiVeID"],
            SuKienID = (int)r["SuKienID"],
            TenLoaiVe = r["TenLoaiVe"].ToString()!,
            MoTa = r["MoTa"] as string,
            DonGia = (decimal)r["DonGia"],
            SoLuongToiDa = (int)r["SoLuongToiDa"],
            SoLuongDaBan = (int)r["SoLuongDaBan"],
            GioiHanMoiKhach = r["GioiHanMoiKhach"] as int?,
            ThoiGianMoBan = r["ThoiGianMoBan"] as DateTime?,
            ThoiGianDongBan = r["ThoiGianDongBan"] as DateTime?,
            TrangThai = (bool)r["TrangThai"]
        };
    }
}

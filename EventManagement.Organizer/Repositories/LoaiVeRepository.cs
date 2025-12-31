using EventManagement.Organizer.Data;
using EventManagement.Organizer.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using EventManagement.Organizer.Repositories.Interfaces;

namespace EventManagement.Organizer.Repositories
{
    public class LoaiVeRepository:ILoaiVeRepository
    {
        private static LoaiVe Map(DataRow r) => new()
        {
            LoaiVeID = Convert.ToInt32(r["LoaiVeID"]),
            SuKienID = Convert.ToInt32(r["SuKienID"]),
            TenLoaiVe = r["TenLoaiVe"].ToString() ?? "",
            MoTa = r["MoTa"] == DBNull.Value ? null : r["MoTa"].ToString(),
            DonGia = Convert.ToDecimal(r["DonGia"]),
            SoLuongToiDa = Convert.ToInt32(r["SoLuongToiDa"]),
            SoLuongDaBan = Convert.ToInt32(r["SoLuongDaBan"]),
            GioiHanMoiKhach = r["GioiHanMoiKhach"] == DBNull.Value ? null : Convert.ToInt32(r["GioiHanMoiKhach"]),
            ThoiGianMoBan = r["ThoiGianMoBan"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiGianMoBan"]),
            ThoiGianDongBan = r["ThoiGianDongBan"] == DBNull.Value ? null : Convert.ToDateTime(r["ThoiGianDongBan"]),
            TrangThai = Convert.ToBoolean(r["TrangThai"])
        };

        public List<LoaiVe> GetBySuKienId(int suKienId)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT LoaiVeID, SuKienID, TenLoaiVe, MoTa, DonGia,
                       SoLuongToiDa, SoLuongDaBan, GioiHanMoiKhach,
                       ThoiGianMoBan, ThoiGianDongBan, TrangThai
                FROM dbo.LoaiVe
                WHERE SuKienID = @id AND TrangThai = 1
                ORDER BY LoaiVeID DESC;",
                new SqlParameter("@id", suKienId));

            var list = new List<LoaiVe>();
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public LoaiVe? GetById(int id)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT LoaiVeID, SuKienID, TenLoaiVe, MoTa, DonGia,
                       SoLuongToiDa, SoLuongDaBan, GioiHanMoiKhach,
                       ThoiGianMoBan, ThoiGianDongBan, TrangThai
                FROM dbo.LoaiVe
                WHERE LoaiVeID = @id;",
                new SqlParameter("@id", id));

            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public int Insert(LoaiVe v)
        {
            var obj = DataHelper.ExecuteScalar(@"
                INSERT INTO dbo.LoaiVe
                    (SuKienID, TenLoaiVe, MoTa, DonGia, SoLuongToiDa, SoLuongDaBan,
                     GioiHanMoiKhach, ThoiGianMoBan, ThoiGianDongBan, TrangThai)
                VALUES
                    (@SuKienID, @Ten, @MoTa, @Gia, @Max, 0,
                     @Limit, @Open, @Close, @TrangThai);

                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new SqlParameter("@SuKienID", v.SuKienID),
                new SqlParameter("@Ten", v.TenLoaiVe),
                new SqlParameter("@MoTa", (object?)v.MoTa ?? DBNull.Value),
                new SqlParameter("@Gia", v.DonGia),
                new SqlParameter("@Max", v.SoLuongToiDa),
                new SqlParameter("@Limit", (object?)v.GioiHanMoiKhach ?? DBNull.Value),
                new SqlParameter("@Open", (object?)v.ThoiGianMoBan ?? DBNull.Value),
                new SqlParameter("@Close", (object?)v.ThoiGianDongBan ?? DBNull.Value),
                new SqlParameter("@TrangThai", v.TrangThai)
            );

            return Convert.ToInt32(obj);
        }

        public int Update(LoaiVe v)
        {
            return DataHelper.ExecuteNonQuery(@"
                UPDATE dbo.LoaiVe
                SET TenLoaiVe = @Ten,
                    MoTa = @MoTa,
                    DonGia = @Gia,
                    SoLuongToiDa = @Max,
                    GioiHanMoiKhach = @Limit,
                    ThoiGianMoBan = @Open,
                    ThoiGianDongBan = @Close,
                    TrangThai = @TrangThai
                WHERE LoaiVeID = @id;",
                new SqlParameter("@Ten", v.TenLoaiVe),
                new SqlParameter("@MoTa", (object?)v.MoTa ?? DBNull.Value),
                new SqlParameter("@Gia", v.DonGia),
                new SqlParameter("@Max", v.SoLuongToiDa),
                new SqlParameter("@Limit", (object?)v.GioiHanMoiKhach ?? DBNull.Value),
                new SqlParameter("@Open", (object?)v.ThoiGianMoBan ?? DBNull.Value),
                new SqlParameter("@Close", (object?)v.ThoiGianDongBan ?? DBNull.Value),
                new SqlParameter("@TrangThai", v.TrangThai),
                new SqlParameter("@id", v.LoaiVeID)
            );
        }

        public int SoftDelete(int id)
        {
            return DataHelper.ExecuteNonQuery(
                @"UPDATE dbo.LoaiVe SET TrangThai = 0 WHERE LoaiVeID = @id;",
                new SqlParameter("@id", id));
        }
    }
}

using EventManagement.Organizer.Data;
using EventManagement.Organizer.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using EventManagement.Organizer.Repositories.Interfaces;

namespace EventManagement.Organizer.Repositories
{
    public class SuKienRepository:ISuKienRepository
    {
        private static SuKien Map(DataRow r) => new()
        {
            SuKienID = Convert.ToInt32(r["SuKienID"]),
            DanhMucID = Convert.ToInt32(r["DanhMucID"]),
            DiaDiemID = Convert.ToInt32(r["DiaDiemID"]),
            ToChucID = Convert.ToInt32(r["ToChucID"]),
            TenSuKien = r["TenSuKien"].ToString() ?? "",
            MoTa = r["MoTa"] == DBNull.Value ? null : r["MoTa"].ToString(),
            ThoiGianBatDau = Convert.ToDateTime(r["ThoiGianBatDau"]),
            ThoiGianKetThuc = Convert.ToDateTime(r["ThoiGianKetThuc"]),
            TrangThai = Convert.ToBoolean(r["TrangThai"])
        };

        public List<SuKien> GetAll()
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT SuKienID, DanhMucID, DiaDiemID, ToChucID, TenSuKien, MoTa,
                       ThoiGianBatDau, ThoiGianKetThuc, TrangThai
                FROM SuKien
                WHERE TrangThai = 1
                ORDER BY SuKienID DESC");

            var list = new List<SuKien>();
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public SuKien? GetById(int id)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT SuKienID, DanhMucID, DiaDiemID, ToChucID, TenSuKien, MoTa,
                       ThoiGianBatDau, ThoiGianKetThuc, TrangThai
                FROM SuKien WHERE SuKienID=@id", new SqlParameter("@id", id));

            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public int Insert(SuKien sk)
        {
            var obj = DataHelper.ExecuteScalar(@"
                INSERT INTO SuKien(DanhMucID, DiaDiemID, ToChucID, TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc, TrangThai)
                VALUES (@DanhMucID, @DiaDiemID, @ToChucID, @Ten, @MoTa, @BD, @KT, @TrangThai);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new SqlParameter("@DanhMucID", sk.DanhMucID),
                new SqlParameter("@DiaDiemID", sk.DiaDiemID),
                new SqlParameter("@ToChucID", sk.ToChucID),
                new SqlParameter("@Ten", sk.TenSuKien),
                new SqlParameter("@MoTa", (object?)sk.MoTa ?? DBNull.Value),
                new SqlParameter("@BD", sk.ThoiGianBatDau),
                new SqlParameter("@KT", sk.ThoiGianKetThuc),
                new SqlParameter("@TrangThai", sk.TrangThai));

            return Convert.ToInt32(obj);
        }

        public int Update(SuKien sk)
        {
            return DataHelper.ExecuteNonQuery(@"
                UPDATE SuKien
                SET DanhMucID=@DanhMucID, DiaDiemID=@DiaDiemID, ToChucID=@ToChucID,
                    TenSuKien=@Ten, MoTa=@MoTa, ThoiGianBatDau=@BD, ThoiGianKetThuc=@KT, TrangThai=@TrangThai
                WHERE SuKienID=@id",
                new SqlParameter("@DanhMucID", sk.DanhMucID),
                new SqlParameter("@DiaDiemID", sk.DiaDiemID),
                new SqlParameter("@ToChucID", sk.ToChucID),
                new SqlParameter("@Ten", sk.TenSuKien),
                new SqlParameter("@MoTa", (object?)sk.MoTa ?? DBNull.Value),
                new SqlParameter("@BD", sk.ThoiGianBatDau),
                new SqlParameter("@KT", sk.ThoiGianKetThuc),
                new SqlParameter("@TrangThai", sk.TrangThai),
                new SqlParameter("@id", sk.SuKienID));
        }

        public int SoftDelete(int id)
        {
            return DataHelper.ExecuteNonQuery(
                @"UPDATE SuKien SET TrangThai = 0 WHERE SuKienID = @id",
                new SqlParameter("@id", id));
        }
    }
}

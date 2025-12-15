using System.Data;
using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace Events_Management.Repositories
{
    public class DanhMucSuKienRepository: IDanhMucSuKienRepository
    {
        private static DanhMucSuKien Map(DataRow r) => new()
        {
            DanhMucID = Convert.ToInt32(r["DanhMucID"]),
            TenDanhMuc = r["TenDanhMuc"].ToString() ?? "",
            MoTa = r["MoTa"] == DBNull.Value ? null : r["MoTa"].ToString(),
            ThuTuHienThi = r["ThuTuHienThi"] == DBNull.Value ? null : Convert.ToInt32(r["ThuTuHienThi"]),
            TrangThai = Convert.ToBoolean(r["TrangThai"])
        };

        public List<DanhMucSuKien> GetAll()
        {
            var dt = DataHelper.GetDataTable(
                @"SELECT DanhMucID, TenDanhMuc, MoTa, ThuTuHienThi, TrangThai
                  FROM DanhMucSuKien
                  WHERE TrangThai = 1
                  ORDER BY ISNULL(ThuTuHienThi, 999999), DanhMucID DESC");

            var list = new List<DanhMucSuKien>();
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public DanhMucSuKien? GetById(int id)
        {
            var dt = DataHelper.GetDataTable(
                @"SELECT DanhMucID, TenDanhMuc, MoTa, ThuTuHienThi, TrangThai
                  FROM DanhMucSuKien WHERE DanhMucID = @id",
                new SqlParameter("@id", id));

            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public int Insert(DanhMucSuKien dm)
        {
            var obj = DataHelper.ExecuteScalar(@"
                INSERT INTO DanhMucSuKien(TenDanhMuc, MoTa, ThuTuHienThi, TrangThai)
                VALUES (@Ten, @MoTa, @ThuTu, @TrangThai);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new SqlParameter("@Ten", dm.TenDanhMuc),
                new SqlParameter("@MoTa", (object?)dm.MoTa ?? DBNull.Value),
                new SqlParameter("@ThuTu", (object?)dm.ThuTuHienThi ?? DBNull.Value),
                new SqlParameter("@TrangThai", dm.TrangThai));

            return Convert.ToInt32(obj);
        }

        public int Update(DanhMucSuKien dm)
        {
            return DataHelper.ExecuteNonQuery(@"
                UPDATE DanhMucSuKien
                SET TenDanhMuc=@Ten, MoTa=@MoTa, ThuTuHienThi=@ThuTu, TrangThai=@TrangThai
                WHERE DanhMucID=@id",
                new SqlParameter("@Ten", dm.TenDanhMuc),
                new SqlParameter("@MoTa", (object?)dm.MoTa ?? DBNull.Value),
                new SqlParameter("@ThuTu", (object?)dm.ThuTuHienThi ?? DBNull.Value),
                new SqlParameter("@TrangThai", dm.TrangThai),
                new SqlParameter("@id", dm.DanhMucID));
        }

        public int SoftDelete(int id)
        {
            return DataHelper.ExecuteNonQuery(
                @"UPDATE DanhMucSuKien SET TrangThai = 0 WHERE DanhMucID = @id",
                new SqlParameter("@id", id));
        }
    }
}

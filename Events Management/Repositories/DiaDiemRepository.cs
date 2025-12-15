using System.Data;
using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
namespace Events_Management.Repositories
{
    public class DiaDiemRepository : IDiaDiemRepository
    {
        private static DiaDiem Map(DataRow r) => new()
        {
            DiaDiemID = Convert.ToInt32(r["DiaDiemID"]),
            TenDiaDiem = r["TenDiaDiem"].ToString() ?? "",
            DiaChi = r["DiaChi"].ToString() ?? "",
            SucChua = r["SucChua"] == DBNull.Value ? null : Convert.ToInt32(r["SucChua"]),
            MoTa = r["MoTa"] == DBNull.Value ? null : r["MoTa"].ToString(),
            TrangThai = Convert.ToBoolean(r["TrangThai"])
        };

        public List<DiaDiem> GetAll()
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT DiaDiemID, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
                FROM DiaDiem
                WHERE TrangThai = 1
                ORDER BY DiaDiemID DESC");

            var list = new List<DiaDiem>();
            foreach (DataRow r in dt.Rows) list.Add(Map(r));
            return list;
        }

        public DiaDiem? GetById(int id)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT DiaDiemID, TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai
                FROM DiaDiem WHERE DiaDiemID=@id", new SqlParameter("@id", id));

            return dt.Rows.Count == 0 ? null : Map(dt.Rows[0]);
        }

        public int Insert(DiaDiem dd)
        {
            var obj = DataHelper.ExecuteScalar(@"
                INSERT INTO DiaDiem(TenDiaDiem, DiaChi, SucChua, MoTa, TrangThai)
                VALUES (@Ten, @DiaChi, @SucChua, @MoTa, @TrangThai);
                SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new SqlParameter("@Ten", dd.TenDiaDiem),
                new SqlParameter("@DiaChi", dd.DiaChi),
                new SqlParameter("@SucChua", (object?)dd.SucChua ?? DBNull.Value),
                new SqlParameter("@MoTa", (object?)dd.MoTa ?? DBNull.Value),
                new SqlParameter("@TrangThai", dd.TrangThai));

            return Convert.ToInt32(obj);
        }

        public int Update(DiaDiem dd)
        {
            return DataHelper.ExecuteNonQuery(@"
                UPDATE DiaDiem
                SET TenDiaDiem=@Ten, DiaChi=@DiaChi, SucChua=@SucChua, MoTa=@MoTa, TrangThai=@TrangThai
                WHERE DiaDiemID=@id",
                new SqlParameter("@Ten", dd.TenDiaDiem),
                new SqlParameter("@DiaChi", dd.DiaChi),
                new SqlParameter("@SucChua", (object?)dd.SucChua ?? DBNull.Value),
                new SqlParameter("@MoTa", (object?)dd.MoTa ?? DBNull.Value),
                new SqlParameter("@TrangThai", dd.TrangThai),
                new SqlParameter("@id", dd.DiaDiemID));
        }

        public int SoftDelete(int id)
        {
            return DataHelper.ExecuteNonQuery(
                @"UPDATE DiaDiem SET TrangThai = 0 WHERE DiaDiemID = @id",
                new SqlParameter("@id", id));
        }
    }
}

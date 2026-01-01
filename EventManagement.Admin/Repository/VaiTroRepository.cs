using EventManagement.Admin.Data;
using EventManagement.Admin.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using EventManagement.Admin.Repository.Interfaces;
namespace EventManagement.Admin.Repository
{
    public class VaiTroRepository:IVaiTroRepository
    {
        public List<VaiTro> GetAll(bool? trangThai = null, string? keyword = null)
        {
            var sql = @"
SELECT VaiTroID, TenVaiTro, MoTa, TrangThai
FROM VaiTro
WHERE (@TrangThai IS NULL OR TrangThai = @TrangThai)
  AND (@Keyword IS NULL OR TenVaiTro LIKE N'%' + @Keyword + N'%' OR MoTa LIKE N'%' + @Keyword + N'%')
ORDER BY VaiTroID DESC;";

            var dt = DataHelper.GetDataTable(sql,
                new SqlParameter("@TrangThai", (object?)trangThai ?? DBNull.Value),
                new SqlParameter("@Keyword", string.IsNullOrWhiteSpace(keyword) ? DBNull.Value : (object)keyword!)
            );

            var list = new List<VaiTro>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new VaiTro
                {
                    VaiTroID = Convert.ToInt32(row["VaiTroID"]),
                    TenVaiTro = row["TenVaiTro"].ToString() ?? "",
                    MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"].ToString(),
                    TrangThai = Convert.ToBoolean(row["TrangThai"]) // BIT -> bool
                });
            }
            return list;
        }

        public VaiTro? GetById(int id)
        {
            var sql = @"SELECT VaiTroID, TenVaiTro, MoTa, TrangThai FROM VaiTro WHERE VaiTroID = @id;";
            var dt = DataHelper.GetDataTable(sql, new SqlParameter("@id", id));
            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];
            return new VaiTro
            {
                VaiTroID = Convert.ToInt32(row["VaiTroID"]),
                TenVaiTro = row["TenVaiTro"].ToString() ?? "",
                MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"].ToString(),
                TrangThai = Convert.ToBoolean(row["TrangThai"])
            };
        }

        public int Insert(VaiTro v)
        {
            var sql = @"
INSERT INTO VaiTro (TenVaiTro, MoTa, TrangThai)
VALUES (@TenVaiTro, @MoTa, @TrangThai);
SELECT SCOPE_IDENTITY();";

            var obj = DataHelper.ExecuteScalar(sql,
                new SqlParameter("@TenVaiTro", v.TenVaiTro),
                new SqlParameter("@MoTa", (object?)v.MoTa ?? DBNull.Value),
                new SqlParameter("@TrangThai", v.TrangThai)
            );

            return Convert.ToInt32(obj);
        }

        public bool Update(int id, VaiTro v)
        {
            var sql = @"
UPDATE VaiTro
SET TenVaiTro = @TenVaiTro,
    MoTa = @MoTa,
    TrangThai = @TrangThai
WHERE VaiTroID = @id;";

            var n = DataHelper.ExecuteNonQuery(sql,
                new SqlParameter("@TenVaiTro", v.TenVaiTro),
                new SqlParameter("@MoTa", (object?)v.MoTa ?? DBNull.Value),
                new SqlParameter("@TrangThai", v.TrangThai),
                new SqlParameter("@id", id)
            );

            return n > 0;
        }

        // Soft delete để không vướng FK NguoiDung_VaiTro
        public bool Delete(int id)
        {
            var sql = @"UPDATE VaiTro SET TrangThai = 0 WHERE VaiTroID = @id;";
            var n = DataHelper.ExecuteNonQuery(sql, new SqlParameter("@id", id));
            return n > 0;
        }
    }
}

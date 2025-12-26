using Events_Management.Data;
using Events_Management.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Events_Management.Repositories.Interfaces;

namespace Events_Management.Repositories
{
    public class VeRepository:IVeRepository
    {
        public int Insert(Ve ve, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"INSERT INTO dbo.Ve(DonHangID, LoaiVeID, NguoiSoHuuID, MaVe, QrToken, TrangThai)VALUES(@DonHangID, @LoaiVeID, @NguoiSoHuuID, @MaVe, @QrToken, @TrangThai);SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran);

            cmd.Parameters.AddWithValue("@DonHangID", ve.DonHangID);
            cmd.Parameters.AddWithValue("@LoaiVeID", ve.LoaiVeID);
            cmd.Parameters.AddWithValue("@NguoiSoHuuID", ve.NguoiSoHuuID);
            cmd.Parameters.AddWithValue("@MaVe", ve.MaVe);
            cmd.Parameters.AddWithValue("@QrToken", ve.QrToken);
            cmd.Parameters.AddWithValue("@TrangThai", ve.TrangThai);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Ve> GetByDonHangId(int donHangId)
        {
            var dt = DataHelper.GetDataTable(@"SELECT VeID, DonHangID, LoaiVeID, NguoiSoHuuID, MaVe, QrToken, NgayPhatHanh, TrangThaiFROM dbo.VeWHERE DonHangID=@idORDER BY VeID;", new SqlParameter("@id", donHangId));
            var list = new List<Ve>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new Ve
                {
                    VeID = Convert.ToInt32(r["VeID"]),
                    DonHangID = Convert.ToInt32(r["DonHangID"]),
                    LoaiVeID = Convert.ToInt32(r["LoaiVeID"]),
                    NguoiSoHuuID = Convert.ToInt32(r["NguoiSoHuuID"]),
                    MaVe = r["MaVe"].ToString()!,
                    QrToken = r["QrToken"].ToString()!,
                    NgayPhatHanh = Convert.ToDateTime(r["NgayPhatHanh"]),
                    TrangThai = Convert.ToByte(r["TrangThai"])
                });
            }
            return list;
        }
    }
}

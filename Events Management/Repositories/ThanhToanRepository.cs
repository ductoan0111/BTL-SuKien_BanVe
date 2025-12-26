using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories
{
    public class ThanhToanRepository:IThanhToanRepository
    {
        public int Insert(ThanhToan tt, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"INSERT INTO dbo.ThanhToan(DonHangID, MaGiaoDich, PhuongThuc, SoTien, TrangThai, ThoiGianThanhToan, RawResponse)VALUES(@DonHangID, @MaGiaoDich, @PhuongThuc, @SoTien, @TrangThai, @ThoiGianThanhToan, @RawResponse);SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran);
            cmd.Parameters.AddWithValue("@DonHangID", tt.DonHangID);
            cmd.Parameters.AddWithValue("@MaGiaoDich", (object?)tt.MaGiaoDich ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhuongThuc", tt.PhuongThuc);
            cmd.Parameters.AddWithValue("@SoTien", tt.SoTien);
            cmd.Parameters.AddWithValue("@TrangThai", tt.TrangThai);
            cmd.Parameters.AddWithValue("@ThoiGianThanhToan", (object?)tt.ThoiGianThanhToan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RawResponse", (object?)tt.RawResponse ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}

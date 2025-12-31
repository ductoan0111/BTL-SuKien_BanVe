using Microsoft.Data.SqlClient;
using EventManagement.Organizer.Models;
using EventManagement.Organizer.Repositories.Interfaces;
namespace EventManagement.Organizer.Repositories
{
    public class CheckinRepository: ICheckinRepository
    {
        public (int veId, int suKienId, byte trangThai, string maVe, string qrToken)? GetVeByQrToken(
            string qrToken, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"
SELECT TOP 1 
    v.VeID, dh.SuKienID, v.TrangThai, v.MaVe, v.QrToken
FROM dbo.Ve v WITH (UPDLOCK, ROWLOCK)
JOIN dbo.DonHang dh ON dh.DonHangID = v.DonHangID
WHERE v.QrToken = @t;", conn, tran);

            cmd.Parameters.AddWithValue("@t", qrToken);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return (
                Convert.ToInt32(rd["VeID"]),
                Convert.ToInt32(rd["SuKienID"]),
                Convert.ToByte(rd["TrangThai"]),
                rd["MaVe"].ToString()!,
                rd["QrToken"].ToString()!
            );
        }

        public (int veId, int suKienId, byte trangThai, string maVe, string qrToken)? GetVeByMaVe(
            string maVe, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"
SELECT TOP 1 
    v.VeID, dh.SuKienID, v.TrangThai, v.MaVe, v.QrToken
FROM dbo.Ve v WITH (UPDLOCK, ROWLOCK)
JOIN dbo.DonHang dh ON dh.DonHangID = v.DonHangID
WHERE v.MaVe = @m;", conn, tran);

            cmd.Parameters.AddWithValue("@m", maVe);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return (
                Convert.ToInt32(rd["VeID"]),
                Convert.ToInt32(rd["SuKienID"]),
                Convert.ToByte(rd["TrangThai"]),
                rd["MaVe"].ToString()!,
                rd["QrToken"].ToString()!
            );
        }

        public int MarkVeCheckedIn(int veId, SqlConnection conn, SqlTransaction tran)
        {
            // theo SQL: Ve.TrangThai: 0=chưa dùng, 1=đã check-in
            using var cmd = new SqlCommand(@"
UPDATE dbo.Ve
SET TrangThai = 1
WHERE VeID = @id AND TrangThai = 0;", conn, tran);

            cmd.Parameters.AddWithValue("@id", veId);
            return cmd.ExecuteNonQuery();
        }

        public int InsertLog(int veId, int suKienId, int? nhanVienId, bool ketQua, string? ghiChu,
            SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"
INSERT INTO dbo.NhatKyCheckin(VeID, SuKienID, NhanVienID, KetQua, GhiChu)
VALUES(@VeID, @SuKienID, @NhanVienID, @KetQua, @GhiChu);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran);

            cmd.Parameters.AddWithValue("@VeID", veId);
            cmd.Parameters.AddWithValue("@SuKienID", suKienId);
            cmd.Parameters.AddWithValue("@NhanVienID", (object?)nhanVienId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KetQua", ketQua);
            cmd.Parameters.AddWithValue("@GhiChu", (object?)ghiChu ?? DBNull.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}

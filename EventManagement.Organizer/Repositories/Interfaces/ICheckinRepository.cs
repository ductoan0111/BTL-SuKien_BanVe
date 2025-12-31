using System.Data;
using Microsoft.Data.SqlClient;
namespace EventManagement.Organizer.Repositories.Interfaces
{
    public interface ICheckinRepository
    {
        (int veId, int suKienId, byte trangThai, string maVe, string qrToken)? GetVeByQrToken(string qrToken, SqlConnection conn, SqlTransaction tran);
        (int veId, int suKienId, byte trangThai, string maVe, string qrToken)? GetVeByMaVe(string maVe, SqlConnection conn, SqlTransaction tran);

        int MarkVeCheckedIn(int veId, SqlConnection conn, SqlTransaction tran);

        int InsertLog(int veId, int suKienId, int? nhanVienId, bool ketQua, string? ghiChu, SqlConnection conn, SqlTransaction tran);
    }
}

using Events_Management.Models;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories.Interfaces
{
    public interface IDonHangRepository
    {
        int InsertDonHang(DonHang dh, SqlConnection conn, SqlTransaction tran);
        int UpdateTongTien(int donHangId, decimal tongTien, SqlConnection conn, SqlTransaction tran);
        int InsertChiTiet(ChiTietDonHang ct, SqlConnection conn, SqlTransaction tran);

        DonHang? GetDonHangById(int id);
        List<ChiTietDonHang> GetChiTietByDonHangId(int donHangId);
        List<DonHang> GetByNguoiMuaId(int nguoiMuaId);
    }
}

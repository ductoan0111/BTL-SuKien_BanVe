using Events_Management.Models;
using Events_Management.Models.DTOs;

namespace Events_Management.Services.Interfaces
{
    public interface IDonHangService
    {
        int Create(CreateDonHangRequest req);
        (DonHang donHang, List<ChiTietDonHang> items) GetDetail(int id);
        List<DonHang> GetByNguoiMua(int nguoiMuaId);
    }
}

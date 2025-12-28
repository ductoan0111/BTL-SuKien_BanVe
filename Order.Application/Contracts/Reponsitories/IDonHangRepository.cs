using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Contracts.Reponsitories
{
    public interface IDonHangRepository
    {
        int Create(DonHang donHang);
        DonHang? GetById(int id);
        List<DonHang> GetByNguoiMua(int nguoiMuaId);
        bool UpdateTrangThai(int donHangId, byte trangThai);
    }
}

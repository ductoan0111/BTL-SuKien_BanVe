using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Contracts.Repositories
{
    public interface IThanhToanRepository
    {
        int Create(ThanhToan thanhToan);
        ThanhToan? GetByDonHang(int donHangId);
        bool UpdateStatus(int thanhToanId, byte trangThai, string? rawResponse = null);
    }
}

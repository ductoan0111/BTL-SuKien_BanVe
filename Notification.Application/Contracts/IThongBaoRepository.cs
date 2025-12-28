using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Contracts
{
    public interface IThongBaoRepository
    {
        int Create(ThongBao thongBao);
        List<ThongBao> GetByNguoiDung(int nguoiDungId);
        bool MarkAsSent(int thongBaoId);
    }
}

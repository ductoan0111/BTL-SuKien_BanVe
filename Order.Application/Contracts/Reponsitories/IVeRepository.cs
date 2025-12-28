using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Contracts.Reponsitories
{
    public interface IVeRepository
    {
        void Create(Ve ve);
        List<Ve> GetByNguoiSoHuu(int nguoiDungId);
        Ve? GetByQrToken(string qrToken);
        bool UpdateTrangThai(int veId, byte trangThai);
    }
}

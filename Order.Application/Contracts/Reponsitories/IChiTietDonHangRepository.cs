using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Contracts.Reponsitories
{
    public interface IChiTietDonHangRepository
    {
        void Add(ChiTietDonHang item);
        List<ChiTietDonHang> GetByDonHang(int donHangId);
    }
}

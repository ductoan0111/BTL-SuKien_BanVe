using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Interface
{
    public interface ILoaiVeRepository
    {
        List<LoaiVe> GetAll();
        List<LoaiVe> GetBySuKien(int suKienId);
        LoaiVe? GetById(int id);
        int Create(LoaiVe entity);
        bool Update(LoaiVe entity);
        bool Delete(int id);
    }
}

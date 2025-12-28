using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Reponsitories
{
    public interface ILoaiVeRepository
    {
        List<LoaiVe> GetBySuKien(int suKienId, bool onlyActive = false);
        LoaiVe? GetById(int id);
        int Create(LoaiVe entity);
        bool Update(LoaiVe entity);
        bool Delete(int id); 
    }
}

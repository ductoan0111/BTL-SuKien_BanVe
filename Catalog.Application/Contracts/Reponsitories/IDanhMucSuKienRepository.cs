using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Reponsitories
{
    public interface IDanhMucSuKienRepository
    {
        List<DanhMucSuKien> GetAll();
        DanhMucSuKien? GetById(int id);
        int Create(DanhMucSuKien entity);
        bool Update(DanhMucSuKien entity);
        bool Delete(int id);
    }
}

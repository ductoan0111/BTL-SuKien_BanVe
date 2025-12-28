using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Reponsitories
{
    public interface ISuKienRepository
    {
        List<SuKien> GetAll(bool onlyActive = false);
        SuKien? GetById(int id);
        int Create(SuKien entity);
        bool Update(SuKien entity);
        bool Delete(int id); 
    }
}

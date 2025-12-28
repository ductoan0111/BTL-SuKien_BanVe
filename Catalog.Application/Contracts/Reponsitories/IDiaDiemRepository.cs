using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Reponsitories
{
    public interface IDiaDiemReponsitory
    {

        List<DiaDiem> GetAll();
        DiaDiem? GetById(int id);
        int Create(DiaDiem entity);
        bool Update(DiaDiem entity);
        bool Delete(int id);
    }
}

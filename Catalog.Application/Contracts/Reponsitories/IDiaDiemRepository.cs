using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Interface
{
    public interface IDiaDiemReponsitory
    {

        List<DiaDiem> GetAll(byte? trangThai = null);
        DiaDiem? GetById(int id);
        int Create(DiaDiem entity);
        bool Update(DiaDiem entity);
        bool UpdateTrangThai(int id, byte trangThai);
        bool Delete(int id);
    }
}

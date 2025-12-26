using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Contracts.Reponsitories
{
    public interface ISuKienToChucRepository
    {
        List<SuKienToChuc> GetAll(); 
        List<SuKienToChuc> GetBySuKien(int suKienId);
        SuKienToChuc? Get(int suKienId, int nguoiDungId);

        bool Create(SuKienToChuc entity);
        bool Update(SuKienToChuc entity);
        bool Delete(int suKienId, int nguoiDungId);

    }
}

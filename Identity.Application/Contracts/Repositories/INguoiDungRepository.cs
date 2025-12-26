using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Repositories
{
    public interface INguoiDungRepository
    {
        List<NguoiDung> GetAll();
        NguoiDung? GetById(int id);
        NguoiDung? GetByEmail(string email);

        int Create(NguoiDung user);
        bool Update(NguoiDung user);
        bool SoftDelete(int id);
    }
}

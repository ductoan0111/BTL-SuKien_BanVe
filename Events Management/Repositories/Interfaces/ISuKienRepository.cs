using System.Collections.Generic;
using Events_Management.Models;
namespace Events_Management.Repositories.Interfaces
{
    public interface ISuKienRepository
    {
        List<SuKien> GetAll();
        SuKien? GetById(int id);
        int Insert(SuKien sk);
        int Update(SuKien sk);
    }
}

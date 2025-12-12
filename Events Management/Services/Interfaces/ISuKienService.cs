using Events_Management.Models;

namespace Events_Management.Services.Interfaces
{
    public interface ISuKienService
    {
        IEnumerable<SuKien> GetAll();
        SuKien? GetById(int id);
        bool Create(SuKien sk, out string message);
        bool Update(SuKien sk, out string message);
    }
}

using EventManagement.Organizer.Models;

namespace EventManagement.Organizer.Services.Interfaces
{
    public interface ISuKienService
    {
        List<SuKien> GetAll();
        SuKien? GetById(int id);
        int Create(SuKien sk);
        bool Update(int id, SuKien sk);
        bool Delete(int id);
    }
}

using EventManagement.Organizer.Models;

namespace EventManagement.Organizer.Repositories.Interfaces
{
    public interface ISuKienRepository
    {
        List<SuKien> GetAll();
        SuKien? GetById(int id);
        int Insert(SuKien sk);
        int Update(SuKien sk);
        int SoftDelete(int id);
    }
}

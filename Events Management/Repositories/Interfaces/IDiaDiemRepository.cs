using Events_Management.Models;

namespace Events_Management.Repositories.Interfaces
{
    public interface IDiaDiemRepository
    {
        List<DiaDiem> GetAll();
        DiaDiem? GetById(int id);
        int Insert(DiaDiem dd);
        int Update(DiaDiem dd);
        int SoftDelete(int id);
    }
}

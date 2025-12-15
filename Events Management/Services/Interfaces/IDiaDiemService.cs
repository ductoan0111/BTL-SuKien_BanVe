using Events_Management.Models;

namespace Events_Management.Services.Interfaces
{
    public interface IDiaDiemService
    {
        List<DiaDiem> GetAll();
        DiaDiem? GetById(int id);
        int Create(DiaDiem dd);
        bool Update(int id, DiaDiem dd);
        bool Delete(int id); // soft delete
    }
}

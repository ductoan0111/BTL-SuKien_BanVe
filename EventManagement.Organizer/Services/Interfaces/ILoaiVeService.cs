using EventManagement.Organizer.Models;
using EventManagement.Organizer.Models.DTOs;

namespace EventManagement.Organizer.Services.Interfaces
{
    public interface ILoaiVeService
    {
        List<LoaiVe> GetBySuKienId(int suKienId);
        LoaiVe? GetById(int id);

        int Create(int suKienId, CreateLoaiVeRequests req);
        bool Update(int id, UpdateLoaiVeRequest req);
        bool Delete(int id);
    }
}

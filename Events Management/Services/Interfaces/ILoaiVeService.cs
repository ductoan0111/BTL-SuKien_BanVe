using Events_Management.Models;
using Events_Management.Models.DTOs;

namespace Events_Management.Services.Interfaces
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

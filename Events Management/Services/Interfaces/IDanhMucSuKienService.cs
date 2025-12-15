using Events_Management.Models;

namespace Events_Management.Services.Interfaces
{
    public interface IDanhMucSuKienService
    {
        List<DanhMucSuKien> GetAll();
        DanhMucSuKien? GetById(int id);
        int Create(DanhMucSuKien dm);
        bool Update(int id, DanhMucSuKien dm);
        bool Delete(int id); // soft delete
    }
}

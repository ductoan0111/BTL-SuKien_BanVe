using Events_Management.Models;

namespace Events_Management.Repositories.Interfaces
{
    public interface IDanhMucSuKienRepository
    {
        List<DanhMucSuKien> GetAll();
        DanhMucSuKien? GetById(int id);
        int Insert(DanhMucSuKien dm);
        int Update(DanhMucSuKien dm);
        int SoftDelete(int id);
    }
}

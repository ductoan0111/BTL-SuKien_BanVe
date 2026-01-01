using EventManagement.Admin.Models;

namespace EventManagement.Admin.Repository.Interfaces
{
    public interface IVaiTroRepository
    {
        List<VaiTro> GetAll(bool? trangThai = null, string? keyword = null);
        VaiTro? GetById(int id);
        int Insert(VaiTro v);
        bool Update(int id, VaiTro v);
        bool Delete(int id); // soft delete: TrangThai = 0
    }
}

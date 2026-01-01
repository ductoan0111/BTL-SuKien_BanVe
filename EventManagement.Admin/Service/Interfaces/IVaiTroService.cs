using EventManagement.Admin.Models;

namespace EventManagement.Admin.Service.Interfaces
{
    public interface IVaiTroService
    {
        List<VaiTro> GetAll(bool? trangThai = null, string? keyword = null);
        VaiTro? GetById(int id);
        int Create(VaiTro v);
        bool Update(int id, VaiTro v);
        bool Delete(int id);
    }
}

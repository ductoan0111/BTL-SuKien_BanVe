using Catalog.Domain.Entities;

namespace Catalog.Application.Contracts.Interface
{
    public interface ISuKienRepository
    {
        List<SuKien> GetAll(bool? trangThai = true); 
        SuKien? GetById(int id, bool? trangThai = true);
        List<SuKien> GetByDiaDiem(int diaDiemId, bool? trangThai = true);

        int Create(SuKien entity);
        bool Update(SuKien entity);

        bool Delete(int id);
    }
}

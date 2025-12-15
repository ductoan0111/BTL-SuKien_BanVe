using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;

namespace Events_Management.Services
{
    public class DanhMucSuKienService : IDanhMucSuKienService
    {
        private readonly IDanhMucSuKienRepository _repo;

        public DanhMucSuKienService(IDanhMucSuKienRepository repo)
        {
            _repo = repo;
        }

        public List<DanhMucSuKien> GetAll() => _repo.GetAll();

        public DanhMucSuKien? GetById(int id) => _repo.GetById(id);

        public int Create(DanhMucSuKien dm)
        {
            if (string.IsNullOrWhiteSpace(dm.TenDanhMuc))
                throw new ArgumentException("Tên danh mục không được rỗng.");

            dm.TrangThai = true;
            return _repo.Insert(dm);
        }

        public bool Update(int id, DanhMucSuKien dm)
        {
            dm.DanhMucID = id;

            if (string.IsNullOrWhiteSpace(dm.TenDanhMuc))
                throw new ArgumentException("Tên danh mục không được rỗng.");

            return _repo.Update(dm) > 0;
        }

        public bool Delete(int id) => _repo.SoftDelete(id) > 0;
    }
}

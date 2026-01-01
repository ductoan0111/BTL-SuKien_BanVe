using EventManagement.Admin.Models;
using EventManagement.Admin.Repository.Interfaces;
using EventManagement.Admin.Service.Interfaces;
namespace EventManagement.Admin.Service
{
    public class VaiTroService:IVaiTroService
    {
        private readonly IVaiTroRepository _repo;
        public VaiTroService(IVaiTroRepository repo) => _repo = repo;

        public List<VaiTro> GetAll(bool? trangThai = null, string? keyword = null)
            => _repo.GetAll(trangThai, keyword);

        public VaiTro? GetById(int id) => _repo.GetById(id);

        public int Create(VaiTro v)
        {
            if (string.IsNullOrWhiteSpace(v.TenVaiTro))
                throw new ArgumentException("TenVaiTro không được rỗng.");

            // ép chuẩn theo đề tài
            v.TenVaiTro = v.TenVaiTro.Trim().ToUpperInvariant();
            return _repo.Insert(v);
        }

        public bool Update(int id, VaiTro v)
        {
            if (string.IsNullOrWhiteSpace(v.TenVaiTro))
                throw new ArgumentException("TenVaiTro không được rỗng.");

            v.TenVaiTro = v.TenVaiTro.Trim().ToUpperInvariant();
            return _repo.Update(id, v);
        }

        public bool Delete(int id) => _repo.Delete(id);
    }
}

using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;
namespace Events_Management.Services
{
    public class DiaDiemService : IDiaDiemService
    {
        private readonly IDiaDiemRepository _repo;

        public DiaDiemService(IDiaDiemRepository repo)
        {
            _repo = repo;
        }

        public List<DiaDiem> GetAll() => _repo.GetAll();

        public DiaDiem? GetById(int id) => _repo.GetById(id);

        public int Create(DiaDiem dd)
        {
            if (string.IsNullOrWhiteSpace(dd.TenDiaDiem))
                throw new ArgumentException("Tên địa điểm không được rỗng.");

            if (string.IsNullOrWhiteSpace(dd.DiaChi))
                throw new ArgumentException("Địa chỉ không được rỗng.");

            if (dd.SucChua is not null && dd.SucChua < 0)
                throw new ArgumentException("Sức chứa không hợp lệ.");

            dd.TrangThai = true;
            return _repo.Insert(dd);
        }

        public bool Update(int id, DiaDiem dd)
        {
            dd.DiaDiemID = id;

            if (string.IsNullOrWhiteSpace(dd.TenDiaDiem))
                throw new ArgumentException("Tên địa điểm không được rỗng.");

            if (string.IsNullOrWhiteSpace(dd.DiaChi))
                throw new ArgumentException("Địa chỉ không được rỗng.");

            if (dd.SucChua is not null && dd.SucChua < 0)
                throw new ArgumentException("Sức chứa không hợp lệ.");

            return _repo.Update(dd) > 0;
        }

        public bool Delete(int id) => _repo.SoftDelete(id) > 0;
    }
}

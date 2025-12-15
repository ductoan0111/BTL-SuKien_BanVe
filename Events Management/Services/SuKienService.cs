using System.Collections.Generic;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;
namespace Events_Management.Services
{
    public class SuKienService : ISuKienService
    {
        private readonly ISuKienRepository _repo;

        public SuKienService(ISuKienRepository repo) => _repo = repo;

        public List<SuKien> GetAll() => _repo.GetAll();

        public SuKien? GetById(int id) => _repo.GetById(id);

        public int Create(SuKien sk)
        {
            if (string.IsNullOrWhiteSpace(sk.TenSuKien))
                throw new ArgumentException("Tên sự kiện không được rỗng.");

            if (sk.ThoiGianKetThuc <= sk.ThoiGianBatDau)
                throw new ArgumentException("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

            return _repo.Insert(sk);
        }

        public bool Update(int id, SuKien sk)
        {
            sk.SuKienID = id;

            if (string.IsNullOrWhiteSpace(sk.TenSuKien))
                throw new ArgumentException("Tên sự kiện không được rỗng.");

            if (sk.ThoiGianKetThuc <= sk.ThoiGianBatDau)
                throw new ArgumentException("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

            return _repo.Update(sk) > 0;
        }

        public bool Delete(int id) => _repo.SoftDelete(id) > 0;
    }
}

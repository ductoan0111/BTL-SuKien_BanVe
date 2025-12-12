using System.Collections.Generic;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;
namespace Events_Management.Services
{
    public class SuKienService : ISuKienService
    {
        private readonly ISuKienRepository _repo;

        public SuKienService(ISuKienRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<SuKien> GetAll() => _repo.GetAll();

        public SuKien? GetById(int id) => _repo.GetById(id);

        public bool Create(SuKien sk, out string message)
        {
            if (string.IsNullOrWhiteSpace(sk.TenSuKien))
            {
                message = "Tên sự kiện không được để trống.";
                return false;
            }

            if (sk.ThoiGianKetThuc <= sk.ThoiGianBatDau)
            {
                message = "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.";
                return false;
            }

            var rows = _repo.Insert(sk);
            if (rows > 0)
            {
                message = "Thêm sự kiện thành công.";
                return true;
            }

            message = "Thêm sự kiện thất bại.";
            return false;
        }

        public bool Update(SuKien sk, out string message)
        {
            if (sk.SuKienID <= 0)
            {
                message = "ID sự kiện không hợp lệ.";
                return false;
            }

            if (sk.ThoiGianKetThuc <= sk.ThoiGianBatDau)
            {
                message = "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.";
                return false;
            }

            var rows = _repo.Update(sk);
            if (rows > 0)
            {
                message = "Cập nhật sự kiện thành công.";
                return true;
            }

            message = "Cập nhật sự kiện thất bại.";
            return false;
        }
    }
}

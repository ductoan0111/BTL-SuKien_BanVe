using EventManagement.Organizer.Models;
using EventManagement.Organizer.Models.DTOs;
using EventManagement.Organizer.Repositories.Interfaces;
using EventManagement.Organizer.Services.Interfaces;

namespace EventManagement.Organizer.Services
{
    public class LoaiVeService: ILoaiVeService
    {
        private readonly ILoaiVeRepository _repo;
        public LoaiVeService(ILoaiVeRepository repo) => _repo = repo;

        public List<LoaiVe> GetBySuKienId(int suKienId) => _repo.GetBySuKienId(suKienId);
        public LoaiVe? GetById(int id) => _repo.GetById(id);

        public int Create(int suKienId, CreateLoaiVeRequests req)
        {
            Validate(req, soLuongDaBanHienTai: 0);

            var v = new LoaiVe
            {
                SuKienID = suKienId,
                TenLoaiVe = req.TenLoaiVe.Trim(),
                MoTa = req.MoTa,
                DonGia = req.DonGia,
                SoLuongToiDa = req.SoLuongToiDa,
                SoLuongDaBan = 0,
                GioiHanMoiKhach = req.GioiHanMoiKhach,
                ThoiGianMoBan = req.ThoiGianMoBan,
                ThoiGianDongBan = req.ThoiGianDongBan,
                TrangThai = req.TrangThai
            };

            return _repo.Insert(v);
        }

        public bool Update(int id, UpdateLoaiVeRequest req)
        {
            var current = _repo.GetById(id);
            if (current == null) return false;

            Validate(req, soLuongDaBanHienTai: current.SoLuongDaBan);

            current.TenLoaiVe = req.TenLoaiVe.Trim();
            current.MoTa = req.MoTa;
            current.DonGia = req.DonGia;
            current.SoLuongToiDa = req.SoLuongToiDa;
            current.GioiHanMoiKhach = req.GioiHanMoiKhach;
            current.ThoiGianMoBan = req.ThoiGianMoBan;
            current.ThoiGianDongBan = req.ThoiGianDongBan;
            current.TrangThai = req.TrangThai;

            return _repo.Update(current) > 0;
        }

        public bool Delete(int id) => _repo.SoftDelete(id) > 0;

        private static void Validate(CreateLoaiVeRequests req, int soLuongDaBanHienTai)
        {
            if (string.IsNullOrWhiteSpace(req.TenLoaiVe))
                throw new ArgumentException("TenLoaiVe không được rỗng.");

            if (req.DonGia < 0)
                throw new ArgumentException("DonGia phải >= 0.");

            if (req.SoLuongToiDa <= 0)
                throw new ArgumentException("SoLuongToiDa phải > 0.");

            if (req.SoLuongToiDa < soLuongDaBanHienTai)
                throw new ArgumentException($"SoLuongToiDa không được nhỏ hơn SoLuongDaBan hiện tại ({soLuongDaBanHienTai}).");

            if (req.GioiHanMoiKhach is not null && req.GioiHanMoiKhach <= 0)
                throw new ArgumentException("GioiHanMoiKhach nếu có thì phải > 0.");

            if (req.ThoiGianMoBan is not null && req.ThoiGianDongBan is not null
                && req.ThoiGianDongBan <= req.ThoiGianMoBan)
                throw new ArgumentException("ThoiGianDongBan phải > ThoiGianMoBan.");
        }
    }
}

using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Models.DTOs;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;

namespace Events_Management.Services
{
    public class DonHangService:IDonHangService
    {
        private readonly IDonHangRepository _donRepo;
        private readonly ILoaiVeRepository _loaiVeRepo;

        public DonHangService(IDonHangRepository donRepo, ILoaiVeRepository loaiVeRepo)
        {
            _donRepo = donRepo;
            _loaiVeRepo = loaiVeRepo;
        }

        public int Create(CreateDonHangRequest req)
        {
            if (req.NguoiMuaID <= 0) throw new ArgumentException("NguoiMuaID không hợp lệ.");
            if (req.SuKienID <= 0) throw new ArgumentException("SuKienID không hợp lệ.");
            if (req.Items == null || req.Items.Count == 0) throw new ArgumentException("Items rỗng.");
            if (req.Items.Any(i => i.LoaiVeID <= 0 || i.SoLuong <= 0))
                throw new ArgumentException("LoaiVeID/SoLuong không hợp lệ.");

            // chống trùng LoaiVeID trong 1 đơn (optional nhưng nên có)
            if (req.Items.GroupBy(i => i.LoaiVeID).Any(g => g.Count() > 1))
                throw new ArgumentException("Items bị trùng LoaiVeID. Hãy gộp số lượng.");

            using var conn = DataHelper.GetConnection();
            using var tran = conn.BeginTransaction();

            try
            {
                var dh = new DonHang { NguoiMuaID = req.NguoiMuaID, SuKienID = req.SuKienID };
                int donHangId = _donRepo.InsertDonHang(dh, conn, tran);

                decimal tongTien = 0;

                foreach (var item in req.Items)
                {
                    var loaiVe = _loaiVeRepo.GetById(item.LoaiVeID);
                    if (loaiVe == null) throw new Exception($"LoaiVeID={item.LoaiVeID} không tồn tại.");
                    if (!loaiVe.TrangThai) throw new Exception($"LoaiVeID={item.LoaiVeID} đã ngưng bán.");
                    if (loaiVe.SuKienID != req.SuKienID) throw new Exception("Loại vé không thuộc sự kiện này.");

                    // optional: giới hạn mua
                    if (loaiVe.GioiHanMoiKhach.HasValue && item.SoLuong > loaiVe.GioiHanMoiKhach.Value)
                        throw new Exception($"Vượt giới hạn mua cho {loaiVe.TenLoaiVe}.");

                    var ct = new ChiTietDonHang
                    {
                        DonHangID = donHangId,
                        LoaiVeID = item.LoaiVeID,
                        SoLuong = item.SoLuong,
                        DonGia = loaiVe.DonGia
                    };

                    _donRepo.InsertChiTiet(ct, conn, tran);
                    tongTien += ct.ThanhTien;
                }

                _donRepo.UpdateTongTien(donHangId, tongTien, conn, tran);

                tran.Commit();
                return donHangId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public (DonHang donHang, List<ChiTietDonHang> items) GetDetail(int id)
        {
            var dh = _donRepo.GetDonHangById(id);
            if (dh == null) throw new KeyNotFoundException("Không tìm thấy đơn hàng.");

            var items = _donRepo.GetChiTietByDonHangId(id);
            return (dh, items);
        }

        public List<DonHang> GetByNguoiMua(int nguoiMuaId)
        {
            if (nguoiMuaId <= 0) throw new ArgumentException("NguoiMuaId không hợp lệ.");
            return _donRepo.GetByNguoiMuaId(nguoiMuaId);
        }
    }
}

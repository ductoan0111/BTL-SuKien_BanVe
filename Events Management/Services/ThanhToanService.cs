using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Models.DTOs;
using Events_Management.Repositories.Interfaces;
using Events_Management.Services.Interfaces;

namespace Events_Management.Services
{
    public class ThanhToanService:IThanhToanService
    {
        private readonly IDonHangRepository _donRepo;
        private readonly ILoaiVeRepository _loaiVeRepo;
        private readonly IThanhToanRepository _ttRepo;
        private readonly IVeRepository _veRepo;

        public ThanhToanService(
            IDonHangRepository donRepo,
            ILoaiVeRepository loaiVeRepo,
            IThanhToanRepository ttRepo,
            IVeRepository veRepo)
        {
            _donRepo = donRepo;
            _loaiVeRepo = loaiVeRepo;
            _ttRepo = ttRepo;
            _veRepo = veRepo;
        }

        public object MockSuccess(int donHangId, MockThanhToanRequest? req)
        {
            // 1) đọc đơn + items (ngoài transaction cũng được, nhưng an toàn thì để trong)
            var dh = _donRepo.GetDonHangById(donHangId);
            if (dh == null) throw new KeyNotFoundException("Không tìm thấy đơn hàng.");

            if (dh.TrangThai != 0)
                throw new InvalidOperationException("Đơn hàng không ở trạng thái chờ thanh toán.");

            var items = _donRepo.GetChiTietByDonHangId(donHangId);
            if (items.Count == 0) throw new InvalidOperationException("Đơn hàng không có item.");

            using var conn = DataHelper.GetConnection(); // lưu ý: DataHelper của bạn đang Open sẵn
            using var tran = conn.BeginTransaction();

            try
            {
                // 2) tăng sold từng loại vé (có check số lượng) => chống oversell
                foreach (var it in items)
                {
                    int ok = _loaiVeRepo.IncreaseSold(it.LoaiVeID, it.SoLuong, conn, tran);
                    if (ok == 0)
                        throw new Exception($"Loại vé {it.LoaiVeID} không đủ số lượng hoặc đã ngưng bán.");
                }

                // 3) update DonHang -> đã thanh toán
                {
                    using var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"UPDATE dbo.DonHang SET TrangThai = 1WHERE DonHangID = @id AND TrangThai = 0;", conn, tran);

                    cmd.Parameters.AddWithValue("@id", donHangId);

                    var affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                        throw new Exception("Không thể cập nhật trạng thái đơn hàng (đơn có thể đã được thanh toán).");
                }

                // 4) insert ThanhToan success
                var maGd = $"MOCK-{donHangId}-{Guid.NewGuid():N}";
                var tt = new ThanhToan
                {
                    DonHangID = donHangId,
                    MaGiaoDich = maGd,
                    PhuongThuc = string.IsNullOrWhiteSpace(req?.PhuongThuc) ? "MOCK" : req!.PhuongThuc!,
                    SoTien = dh.TongTien,
                    TrangThai = 1,
                    ThoiGianThanhToan = DateTime.Now,
                    RawResponse = req?.RawResponse
                };
                int thanhToanId = _ttRepo.Insert(tt, conn, tran);

                // 5) sinh vé: mỗi item -> SoLuong vé
                int soVe = 0;
                foreach (var it in items)
                {
                    for (int i = 0; i < it.SoLuong; i++)
                    {
                        var ve = new Ve
                        {
                            DonHangID = donHangId,
                            LoaiVeID = it.LoaiVeID,
                            NguoiSoHuuID = dh.NguoiMuaID,
                            MaVe = $"VE-{donHangId}-{it.LoaiVeID}-{Guid.NewGuid():N}".Substring(0, 35),
                            QrToken = Guid.NewGuid().ToString("N"),
                            TrangThai = 0
                        };
                        _veRepo.Insert(ve, conn, tran);
                        soVe++;
                    }
                }

                tran.Commit();

                return new
                {
                    DonHangID = donHangId,
                    ThanhToanID = thanhToanId,
                    MaGiaoDich = maGd,
                    SoVe = soVe
                };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}

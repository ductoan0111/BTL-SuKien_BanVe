using EventManagement.Organizer.Data;
using EventManagement.Organizer.Models.DTOs;
using EventManagement.Organizer.Repositories.Interfaces;
using EventManagement.Organizer.Services.Interfaces;

namespace EventManagement.Organizer.Services
{
    public class CheckinService: ICheckinService
    {
        private readonly ICheckinRepository _repo;

        public CheckinService(ICheckinRepository repo) => _repo = repo;

        public object Checkin(CheckinRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.QrToken) && string.IsNullOrWhiteSpace(req.MaVe))
                throw new ArgumentException("Cần QrToken hoặc MaVe.");

            using var conn = DataHelper.GetConnection(); // theo project bạn: thường đã Open sẵn
            using var tran = conn.BeginTransaction();

            try
            {
                var veInfo = !string.IsNullOrWhiteSpace(req.QrToken)
                    ? _repo.GetVeByQrToken(req.QrToken!, conn, tran)
                    : _repo.GetVeByMaVe(req.MaVe!, conn, tran);

                if (veInfo == null) throw new KeyNotFoundException("Không tìm thấy vé.");

                var (veId, suKienId, trangThai, maVe, qrToken) = veInfo.Value;

                // 0=chưa dùng, 1=đã check-in
                if (trangThai != 0)
                {
                    // vẫn ghi log thất bại nếu muốn
                    var logFail = _repo.InsertLog(veId, suKienId, req.NhanVienID, false, "Vé đã check-in/không hợp lệ", conn, tran);
                    tran.Commit();
                    return new { Message = "Check-in thất bại", MaVe = maVe, QrToken = qrToken, LogID = logFail };
                }

                var affected = _repo.MarkVeCheckedIn(veId, conn, tran);
                if (affected == 0)
                {
                    var logRace = _repo.InsertLog(veId, suKienId, req.NhanVienID, false, "Vé vừa được check-in trước đó", conn, tran);
                    tran.Commit();
                    return new { Message = "Check-in thất bại", MaVe = maVe, QrToken = qrToken, LogID = logRace };
                }

                var logId = _repo.InsertLog(veId, suKienId, req.NhanVienID, true, req.GhiChu, conn, tran);

                tran.Commit();
                return new { Message = "Check-in thành công", VeID = veId, SuKienID = suKienId, MaVe = maVe, QrToken = qrToken, LogID = logId };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}

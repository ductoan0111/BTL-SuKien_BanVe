using EventManagement.Organizer.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using EventManagement.Organizer.Repositories.Interfaces;

namespace EventManagement.Organizer.Repositories
{
    public class ReportRepository: IReportRepository
    {
        public object Sales(int suKienId)
        {
            // Tổng vé bán + doanh thu
            var sumDt = DataHelper.GetDataTable(@"
SELECT 
    ISNULL(SUM(ct.SoLuong),0) AS VeDaBan,
    ISNULL(SUM(ct.SoLuong * ct.DonGia),0) AS DoanhThu
FROM dbo.DonHang dh
JOIN dbo.ChiTietDonHang ct ON ct.DonHangID = dh.DonHangID
WHERE dh.SuKienID = @id AND dh.TrangThai = 1;",
                new SqlParameter("@id", suKienId));

            var r = sumDt.Rows[0];
            int veDaBan = Convert.ToInt32(r["VeDaBan"]);
            decimal doanhThu = Convert.ToDecimal(r["DoanhThu"]);

            // Breakdown theo loại vé
            var dt = DataHelper.GetDataTable(@"
SELECT 
    lv.LoaiVeID, lv.TenLoaiVe,
    ISNULL(SUM(ct.SoLuong),0) AS VeDaBan,
    ISNULL(SUM(ct.SoLuong * ct.DonGia),0) AS DoanhThu
FROM dbo.DonHang dh
JOIN dbo.ChiTietDonHang ct ON ct.DonHangID = dh.DonHangID
JOIN dbo.LoaiVe lv ON lv.LoaiVeID = ct.LoaiVeID
WHERE dh.SuKienID = @id AND dh.TrangThai = 1
GROUP BY lv.LoaiVeID, lv.TenLoaiVe
ORDER BY lv.LoaiVeID;",
                new SqlParameter("@id", suKienId));

            var breakdown = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                breakdown.Add(new
                {
                    LoaiVeID = Convert.ToInt32(row["LoaiVeID"]),
                    TenLoaiVe = row["TenLoaiVe"].ToString(),
                    VeDaBan = Convert.ToInt32(row["VeDaBan"]),
                    DoanhThu = Convert.ToDecimal(row["DoanhThu"])
                });
            }

            return new { SuKienID = suKienId, VeDaBan = veDaBan, DoanhThu = doanhThu, Breakdown = breakdown };
        }

        public object Attendance(int suKienId)
        {
            var dt = DataHelper.GetDataTable(@"
SELECT
    COUNT(*) AS VePhatHanh,
    SUM(CASE WHEN v.TrangThai = 1 THEN 1 ELSE 0 END) AS DaCheckin
FROM dbo.Ve v
JOIN dbo.DonHang dh ON dh.DonHangID = v.DonHangID
WHERE dh.SuKienID = @id;",
                new SqlParameter("@id", suKienId));

            var r = dt.Rows[0];
            int vePhatHanh = Convert.ToInt32(r["VePhatHanh"]);
            int daCheckin = Convert.ToInt32(r["DaCheckin"]);
            double tiLe = vePhatHanh == 0 ? 0 : Math.Round(daCheckin * 100.0 / vePhatHanh, 2);

            return new { SuKienID = suKienId, VePhatHanh = vePhatHanh, DaCheckin = daCheckin, TiLeThamDu = tiLe };
        }
    }
}

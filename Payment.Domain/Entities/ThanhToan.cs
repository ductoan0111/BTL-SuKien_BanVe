using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Entities
{
    public class ThanhToan
    {
        public int ThanhToanID { get; set; }
        public int DonHangID { get; set; }
        public string? MaGiaoDich { get; set; }
        public string PhuongThuc { get; set; } = string.Empty;
        public decimal SoTien { get; set; }
        public byte TrangThai { get; set; }
        public DateTime? ThoiGianThanhToan { get; set; }
        public string? RawResponse { get; set; }
    }
}

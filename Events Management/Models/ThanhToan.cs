namespace Events_Management.Models
{
    public class ThanhToan
    {
        public int ThanhToanID { get; set; }
        public int DonHangID { get; set; }
        public decimal SoTien { get; set; }
        public DateTime ThoiGianThanhToan { get; set; }
        public string PhuongThuc { get; set; } = null!;
        public string? MaGiaoDichGateway { get; set; }
        public byte TrangThai { get; set; }

        public virtual DonHang? DonHang { get; set; }
    }
}

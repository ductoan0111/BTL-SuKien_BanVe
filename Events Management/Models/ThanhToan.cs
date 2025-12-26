namespace Events_Management.Models
{
    public class ThanhToan
    {
        public int ThanhToanID { get; set; }
        public int DonHangID { get; set; }
        public string? MaGiaoDich { get; set; }
        public string PhuongThuc { get; set; } = "MOCK";
        public decimal SoTien { get; set; }
        public byte TrangThai { get; set; } // 0=KhoiTao, 1=ThanhCong, 2=ThatBai, 3=HoanTien
        public DateTime? ThoiGianThanhToan { get; set; }
        public string? RawResponse { get; set; }
    }
}

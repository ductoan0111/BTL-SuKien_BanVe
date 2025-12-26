namespace Events_Management.Models
{
    public class DonHang
    {
        public int DonHangID { get; set; }
        public int NguoiMuaID { get; set; }
        public int SuKienID { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public byte TrangThai { get; set; } // 0=ChoThanhToan, 1=DaThanhToan, 2=DaHuy...
    }
    public class ChiTietDonHang
    {
        public int ChiTietID { get; set; }
        public int DonHangID { get; set; }
        public int LoaiVeID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}

namespace Events_Management.Models
{
    public class DonHang
    {
        public int DonHangID { get; set; }
        public int KhachID { get; set; }
        public int SuKienID { get; set; }
        public decimal TongTien { get; set; }
        public byte TrangThai { get; set; }
        public DateTime NgayDat { get; set; }

        public virtual NguoiDung? Khach { get; set; }
        public virtual SuKien? SuKien { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
        public virtual ICollection<Ve> Ves { get; set; } = new List<Ve>();
    }
}

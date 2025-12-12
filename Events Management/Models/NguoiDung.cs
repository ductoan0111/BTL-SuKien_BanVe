namespace Events_Management.Models
{
    public class NguoiDung
    {
        public int NguoiDungID { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MatKhauHash { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public int VaiTroID { get; set; }
        public byte TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public virtual VaiTro? VaiTro { get; set; }

        public virtual ICollection<SuKien> SuKiens { get; set; } = new List<SuKien>();
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
        public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
    }
}

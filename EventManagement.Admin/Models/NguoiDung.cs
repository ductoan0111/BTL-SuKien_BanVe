namespace EventManagement.Admin.Models
{
    public class NguoiDung
    {
        public int NguoiDungID { get; set; }
        public int VaiTroID { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhauHash { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }

        // optional cho view
        public string? TenVaiTro { get; set; }
    }
}

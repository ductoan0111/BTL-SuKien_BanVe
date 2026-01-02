namespace EventManagement.Admin.Models.DTOs
{
    public class CreateNguoiDungRequest
    {
        public int VaiTroID { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhauHash { get; set; } // theo SQL: lưu hash
    }
    public class UpdateNguoiDungRequest
    {
        public int VaiTroID { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhauHash { get; set; }
    }
    public class UpdateNguoiDungStatusRequest
    {
        public bool TrangThai { get; set; } // true=active, false=locked
    }
}

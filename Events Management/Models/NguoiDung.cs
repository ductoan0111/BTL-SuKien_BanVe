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
    }
}

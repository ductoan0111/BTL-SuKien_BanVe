namespace EventManagement.Organizer.Models.DTOs
{
    public class CreateLoaiVeRequests
    {
        public string TenLoaiVe { get; set; } = null!;
        public string? MoTa { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongToiDa { get; set; }
        public int? GioiHanMoiKhach { get; set; }
        public DateTime? ThoiGianMoBan { get; set; }
        public DateTime? ThoiGianDongBan { get; set; }
        public bool TrangThai { get; set; } = true;
    }
    public class UpdateLoaiVeRequest : CreateLoaiVeRequests
    {
        // giống Create, có thể bật/tắt TrangThai
    }
}

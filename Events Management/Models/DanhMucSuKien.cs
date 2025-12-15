namespace Events_Management.Models
{
    public class DanhMucSuKien
    {
        public int DanhMucID { get; set; }
        public string TenDanhMuc { get; set; } = null!;
        public string? MoTa { get; set; }
        public int? ThuTuHienThi { get; set; }
        public bool TrangThai { get; set; } = true;
    }
}

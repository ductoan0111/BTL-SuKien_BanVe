namespace Events_Management.Models
{
    public class SuKien
    {
        public int SuKienID { get; set; }
        public int DanhMucID { get; set; }
        public int DiaDiemID { get; set; }
        public int ToChucID { get; set; }
        public string TenSuKien { get; set; } = null!;
        public string? MoTa { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public bool TrangThai { get; set; } = true; // BIT

    }
}

namespace Events_Management.Models
{
    public class DiaDiem
    {
        public int DiaDiemID { get; set; }
        public string TenDiaDiem { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public int? SucChua { get; set; }
        public string? MoTa { get; set; }
        public bool TrangThai { get; set; } = true;

    }
}

namespace Events_Management.Models
{
    public class DiaDiem
    {
        public int DiaDiemID { get; set; }
        public string TenDiaDiem { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public int? SucChua { get; set; }
        public string? MoTa { get; set; }

    }
}

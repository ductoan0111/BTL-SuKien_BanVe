namespace Events_Management.Models
{
    public class VaiTro
    {
        public int VaiTroID { get; set; }
        public string MaVaiTro { get; set; } = null!;
        public string TenVaiTro { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
    }
}

namespace Events_Management.Models
{
    public class ThongBao
    {
        public int ThongBaoID { get; set; }
        public int NguoiNhanID { get; set; }
        public string TieuDe { get; set; } = null!;
        public string NoiDung { get; set; } = null!;
        public string KenhGui { get; set; } = null!;
        public DateTime ThoiGianGui { get; set; }
        public byte TrangThai { get; set; }

        public virtual NguoiDung? NguoiNhan { get; set; }
    }
}

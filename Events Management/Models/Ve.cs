namespace Events_Management.Models
{
    public class Ve
    {
        public int VeID { get; set; }
        public int LoaiVeID { get; set; }
        public int DonHangID { get; set; }
        public string MaVe { get; set; } = null!;
        public string? QRCode { get; set; }
        public byte TrangThai { get; set; }

        public virtual LoaiVe? LoaiVe { get; set; }
        public virtual DonHang? DonHang { get; set; }

        public virtual ICollection<NhatKyCheckin> NhatKyCheckins { get; set; } = new List<NhatKyCheckin>();
    }
}

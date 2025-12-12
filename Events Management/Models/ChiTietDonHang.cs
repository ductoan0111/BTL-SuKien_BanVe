namespace Events_Management.Models
{
    public class ChiTietDonHang
    {
        public int ChiTietID { get; set; }
        public int DonHangID { get; set; }
        public int LoaiVeID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // Cột computed trong SQL: ThanhTien = SoLuong * DonGia
        public decimal? ThanhTien { get; set; }

        public virtual DonHang? DonHang { get; set; }
        public virtual LoaiVe? LoaiVe { get; set; }
    }
}

namespace Events_Management.Models
{
    public class LoaiVe
    {
        public int LoaiVeID { get; set; }
        public int SuKienID { get; set; }
        public string TenLoaiVe { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public int SoLuongToiDa { get; set; }
        public int? GioiHanMoiKhach { get; set; }
    }
}

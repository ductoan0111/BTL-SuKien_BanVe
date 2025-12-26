namespace Events_Management.Models
{
    public class Ve
    {
        public int VeID { get; set; }
        public int DonHangID { get; set; }
        public int LoaiVeID { get; set; }
        public int NguoiSoHuuID { get; set; }
        public string MaVe { get; set; } = null!;
        public string QrToken { get; set; } = null!;
        public DateTime NgayPhatHanh { get; set; }
        public byte TrangThai { get; set; } // 0=Chua su dung, 1=Da checkin, 2=Da huy, 3=Da hoan
    }
}

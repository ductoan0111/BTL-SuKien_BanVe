namespace EventManagement.Organizer.Models
{
    public class NhatKyCheckin
    {
        public int CheckinID { get; set; }
        public int VeID { get; set; }
        public int SuKienID { get; set; }
        public int NhanVienID { get; set; }
        public DateTime ThoiGianCheckin { get; set; }
        public bool KetQua {  get; set; }
        public string? GhiChu { get; set; }
    }
}

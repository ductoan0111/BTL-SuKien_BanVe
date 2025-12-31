namespace EventManagement.Organizer.Models.DTOs
{
    public class CheckinRequest
    {
        // nhập 1 trong 2
        public string? QrToken { get; set; }
        public string? MaVe { get; set; }

        // theo SQL: NhanVienID có thể NULL, nhưng để test bạn cứ nhập
        public int? NhanVienID { get; set; }
        public string? GhiChu { get; set; }
    }
}

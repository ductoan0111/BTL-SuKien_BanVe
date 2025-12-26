namespace Events_Management.Models.DTOs
{
    public class CreateDonHangRequest
    {
        public int NguoiMuaID { get; set; }
        public int SuKienID { get; set; }
        public List<CreateDonHangItem> Items { get; set; } = new();
    }
    public class CreateDonHangItem
    {
        public int LoaiVeID { get; set; }
        public int SoLuong { get; set; }
    }
}

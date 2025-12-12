namespace Events_Management.Models
{
    public class NhatKyCheckin
    {
        public int CheckinID { get; set; }
        public int VeID { get; set; }
        public DateTime ThoiGianCheckin { get; set; }
        public string? ThietBi { get; set; }
        public string? GhiChu { get; set; }

        public virtual Ve? Ve { get; set; }
    }
}

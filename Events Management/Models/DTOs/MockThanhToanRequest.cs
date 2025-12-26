namespace Events_Management.Models.DTOs
{
    public class MockThanhToanRequest
    {
        public string? PhuongThuc { get; set; } = "MOCK";
        public string? RawResponse { get; set; } // muốn lưu log thì gửi
    }
}

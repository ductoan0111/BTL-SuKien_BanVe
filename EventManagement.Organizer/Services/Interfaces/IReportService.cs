namespace EventManagement.Organizer.Services.Interfaces
{
    public interface IReportService
    {
        object Sales(int suKienId);
        object Attendance(int suKienId);
    }
}

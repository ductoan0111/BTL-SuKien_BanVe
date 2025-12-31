namespace EventManagement.Organizer.Repositories.Interfaces
{
    public interface IReportRepository
    {
        object Sales(int suKienId);
        object Attendance(int suKienId);
    }
}

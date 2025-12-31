using EventManagement.Organizer.Repositories.Interfaces;
using EventManagement.Organizer.Services.Interfaces;

namespace EventManagement.Organizer.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _repo;
        public ReportService(IReportRepository repo) => _repo = repo;

        public object Sales(int suKienId) => _repo.Sales(suKienId);
        public object Attendance(int suKienId) => _repo.Attendance(suKienId);
    }
}

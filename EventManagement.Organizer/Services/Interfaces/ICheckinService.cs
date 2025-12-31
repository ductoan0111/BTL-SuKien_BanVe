using EventManagement.Organizer.Models.DTOs;

namespace EventManagement.Organizer.Services.Interfaces
{
    public interface ICheckinService
    {
        object Checkin(CheckinRequest req);
    }
}

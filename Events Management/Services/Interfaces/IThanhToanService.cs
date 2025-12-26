using Events_Management.Models.DTOs;

namespace Events_Management.Services.Interfaces
{
    public interface IThanhToanService
    {
        object MockSuccess(int donHangId, MockThanhToanRequest? req);
    }
}

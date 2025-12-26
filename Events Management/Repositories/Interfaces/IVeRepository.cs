using Events_Management.Models;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories.Interfaces
{
    public interface IVeRepository
    {
        int Insert(Ve ve, SqlConnection conn, SqlTransaction tran);
        List<Ve> GetByDonHangId(int donHangId);
    }
}

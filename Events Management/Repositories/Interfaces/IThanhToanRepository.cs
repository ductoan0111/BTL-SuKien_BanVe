using Events_Management.Models;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories.Interfaces
{
    public interface IThanhToanRepository
    {
        int Insert(ThanhToan tt, SqlConnection conn, SqlTransaction tran);
    }
}

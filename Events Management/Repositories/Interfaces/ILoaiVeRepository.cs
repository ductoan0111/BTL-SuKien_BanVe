using Events_Management.Models;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories.Interfaces
{
    public interface ILoaiVeRepository
    {
        List<LoaiVe> GetBySuKienId(int suKienId);
        LoaiVe? GetById(int id);

        int Insert(LoaiVe v);
        int Update(LoaiVe v);
        int SoftDelete(int id);
        int IncreaseSold(int loaiVeId, int qty, SqlConnection conn, SqlTransaction tran);
    }
}

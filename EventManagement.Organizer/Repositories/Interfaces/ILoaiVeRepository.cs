using EventManagement.Organizer.Models;

namespace EventManagement.Organizer.Repositories.Interfaces
{
    public interface ILoaiVeRepository
    {
        List<LoaiVe> GetBySuKienId(int suKienId);
        LoaiVe? GetById(int id);

        int Insert(LoaiVe v);
        int Update(LoaiVe v);
        int SoftDelete(int id);
    }
}

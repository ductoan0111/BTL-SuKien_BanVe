using BuildingBlocks.Shareds.Abstractions;
using Order.Application.Contracts.Reponsitories;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Reponsitories
{
    public class ChiTietDonHangRepository : IChiTietDonHangRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ChiTietDonHangRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Add(ChiTietDonHang item)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ChiTietDonHang
                (DonHangID, LoaiVeID, SoLuong, DonGia)
                VALUES (@DonHangID, @LoaiVeID, @SoLuong, @DonGia)";

            Add(cmd, "@DonHangID", item.DonHangID);
            Add(cmd, "@LoaiVeID", item.LoaiVeID);
            Add(cmd, "@SoLuong", item.SoLuong);
            Add(cmd, "@DonGia", item.DonGia);

            cmd.ExecuteNonQuery();
        }

        public List<ChiTietDonHang> GetByDonHang(int donHangId)
        {
            var list = new List<ChiTietDonHang>();
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM ChiTietDonHang WHERE DonHangID=@id";
            Add(cmd, "@id", donHangId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new ChiTietDonHang
                {
                    ChiTietID = (int)rd["ChiTietID"],
                    DonHangID = donHangId,
                    LoaiVeID = (int)rd["LoaiVeID"],
                    SoLuong = (int)rd["SoLuong"],
                    DonGia = (decimal)rd["DonGia"]
                });
            }
            return list;
        }

        private void Add(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            cmd.Parameters.Add(p);
        }
    }
}

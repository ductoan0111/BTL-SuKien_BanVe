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
    public class DonHangRepository : IDonHangRepository
    {
        private readonly IDbConnectionFactory _factory;

        public DonHangRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public int Create(DonHang dh)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO DonHang(NguoiMuaID, SuKienID, TongTien, TrangThai)
                OUTPUT INSERTED.DonHangID
                VALUES(@NguoiMuaID, @SuKienID, @TongTien, @TrangThai)";

            Add(cmd, "@NguoiMuaID", dh.NguoiMuaID);
            Add(cmd, "@SuKienID", dh.SuKienID);
            Add(cmd, "@TongTien", dh.TongTien);
            Add(cmd, "@TrangThai", dh.TrangThai);

            return (int)cmd.ExecuteScalar()!;
        }

        public DonHang? GetById(int id)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM DonHang WHERE DonHangID=@id";
            Add(cmd, "@id", id);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return new DonHang
            {
                DonHangID = id,
                NguoiMuaID = (int)rd["NguoiMuaID"],
                SuKienID = (int)rd["SuKienID"],
                TongTien = (decimal)rd["TongTien"],
                TrangThai = (byte)rd["TrangThai"],
                NgayDat = (DateTime)rd["NgayDat"]
            };
        }

        public List<DonHang> GetByNguoiMua(int nguoiMuaId)
        {
            var list = new List<DonHang>();
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM DonHang WHERE NguoiMuaID=@uid";
            Add(cmd, "@uid", nguoiMuaId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new DonHang
                {
                    DonHangID = (int)rd["DonHangID"],
                    NguoiMuaID = nguoiMuaId,
                    SuKienID = (int)rd["SuKienID"],
                    TongTien = (decimal)rd["TongTien"],
                    TrangThai = (byte)rd["TrangThai"],
                    NgayDat = (DateTime)rd["NgayDat"]
                });
            }
            return list;
        }

        public bool UpdateTrangThai(int id, byte trangThai)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE DonHang SET TrangThai=@st WHERE DonHangID=@id";
            Add(cmd, "@st", trangThai);
            Add(cmd, "@id", id);

            return cmd.ExecuteNonQuery() > 0;
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


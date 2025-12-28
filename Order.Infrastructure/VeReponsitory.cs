using BuildingBlocks.Shareds.Abstractions;
using Order.Application.Contracts.Reponsitories;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure
{
    public class VeRepository : IVeRepository
    {
        private readonly IDbConnectionFactory _factory;

        public VeRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Create(Ve ve)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Ve
                (DonHangID, LoaiVeID, NguoiSoHuuID, MaVe, QrToken)
                VALUES (@DonHangID, @LoaiVeID, @NguoiSoHuuID, @MaVe, @QrToken)";

            Add(cmd, "@DonHangID", ve.DonHangID);
            Add(cmd, "@LoaiVeID", ve.LoaiVeID);
            Add(cmd, "@NguoiSoHuuID", ve.NguoiSoHuuID);
            Add(cmd, "@MaVe", ve.MaVe);
            Add(cmd, "@QrToken", ve.QrToken);

            cmd.ExecuteNonQuery();
        }

        public List<Ve> GetByNguoiSoHuu(int nguoiDungId)
        {
            var list = new List<Ve>();
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ve WHERE NguoiSoHuuID=@uid";
            Add(cmd, "@uid", nguoiDungId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }

        public Ve? GetByQrToken(string qrToken)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ve WHERE QrToken=@qr";
            Add(cmd, "@qr", qrToken);

            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        public bool UpdateTrangThai(int veId, byte trangThai)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Ve SET TrangThai=@st WHERE VeID=@id";
            Add(cmd, "@st", trangThai);
            Add(cmd, "@id", veId);

            return cmd.ExecuteNonQuery() > 0;
        }

        private Ve Map(IDataReader rd) => new()
        {
            VeID = (int)rd["VeID"],
            DonHangID = (int)rd["DonHangID"],
            LoaiVeID = (int)rd["LoaiVeID"],
            NguoiSoHuuID = (int)rd["NguoiSoHuuID"],
            MaVe = rd["MaVe"].ToString()!,
            QrToken = rd["QrToken"].ToString()!,
            NgayPhatHanh = (DateTime)rd["NgayPhatHanh"],
            TrangThai = (byte)rd["TrangThai"]
        };

        private void Add(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            cmd.Parameters.Add(p);
        }
    }
}

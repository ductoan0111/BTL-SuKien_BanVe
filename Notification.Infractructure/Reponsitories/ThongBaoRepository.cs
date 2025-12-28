using BuildingBlocks.Shareds.Abstractions;
using Notification.Application.Contracts;
using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infractructure.Reponsitories
{
    public class ThongBaoRepository : IThongBaoRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ThongBaoRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public int Create(ThongBao tb)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ThongBao
                (NguoiDungID, DonHangID, VeID, LoaiThongBao, TieuDe, NoiDung, TrangThai)
                OUTPUT INSERTED.ThongBaoID
                VALUES
                (@NguoiDungID, @DonHangID, @VeID, @Loai, @TieuDe, @NoiDung, @TrangThai)";

            Add(cmd, "@NguoiDungID", tb.NguoiDungID);
            Add(cmd, "@DonHangID", tb.DonHangID);
            Add(cmd, "@VeID", tb.VeID);
            Add(cmd, "@Loai", tb.LoaiThongBao);
            Add(cmd, "@TieuDe", tb.TieuDe);
            Add(cmd, "@NoiDung", tb.NoiDung);
            Add(cmd, "@TrangThai", tb.TrangThai);

            return (int)cmd.ExecuteScalar()!;
        }

        public List<ThongBao> GetByNguoiDung(int nguoiDungId)
        {
            var list = new List<ThongBao>();
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM ThongBao WHERE NguoiDungID=@uid ORDER BY ThoiGianTao DESC";
            Add(cmd, "@uid", nguoiDungId);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new ThongBao
                {
                    ThongBaoID = (int)rd["ThongBaoID"],
                    NguoiDungID = nguoiDungId,
                    DonHangID = rd["DonHangID"] as int?,
                    VeID = rd["VeID"] as int?,
                    LoaiThongBao = rd["LoaiThongBao"].ToString()!,
                    TieuDe = rd["TieuDe"].ToString()!,
                    NoiDung = rd["NoiDung"].ToString()!,
                    TrangThai = (byte)rd["TrangThai"],
                    ThoiGianTao = (DateTime)rd["ThoiGianTao"],
                    ThoiGianGui = rd["ThoiGianGui"] as DateTime?,
                    GhiChu = rd["GhiChu"] as string
                });
            }
            return list;
        }

        public bool MarkAsSent(int thongBaoId)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE ThongBao
                SET TrangThai=1, ThoiGianGui=SYSDATETIME()
                WHERE ThongBaoID=@id";

            Add(cmd, "@id", thongBaoId);
            return cmd.ExecuteNonQuery() > 0;
        }

        private void Add(IDbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}

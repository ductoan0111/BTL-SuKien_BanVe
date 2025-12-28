using BuildingBlocks.Shareds.Abstractions;
using Payment.Application.Contracts.Repositories;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ThanhToanRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public int Create(ThanhToan tt)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ThanhToan
                (DonHangID, MaGiaoDich, PhuongThuc, SoTien, TrangThai)
                OUTPUT INSERTED.ThanhToanID
                VALUES (@DonHangID, @MaGiaoDich, @PhuongThuc, @SoTien, @TrangThai)";

            Add(cmd, "@DonHangID", tt.DonHangID);
            Add(cmd, "@MaGiaoDich", tt.MaGiaoDich);
            Add(cmd, "@PhuongThuc", tt.PhuongThuc);
            Add(cmd, "@SoTien", tt.SoTien);
            Add(cmd, "@TrangThai", tt.TrangThai);

            return (int)cmd.ExecuteScalar()!;
        }

        public ThanhToan? GetByDonHang(int donHangId)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM ThanhToan WHERE DonHangID=@id";
            Add(cmd, "@id", donHangId);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return new ThanhToan
            {
                ThanhToanID = (int)rd["ThanhToanID"],
                DonHangID = donHangId,
                MaGiaoDich = rd["MaGiaoDich"] as string,
                PhuongThuc = rd["PhuongThuc"].ToString()!,
                SoTien = (decimal)rd["SoTien"],
                TrangThai = (byte)rd["TrangThai"],
                ThoiGianThanhToan = rd["ThoiGianThanhToan"] as DateTime?,
                RawResponse = rd["RawResponse"] as string
            };
        }

        public bool UpdateStatus(int thanhToanId, byte trangThai, string? rawResponse = null)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE ThanhToan
                SET TrangThai=@TrangThai,
                    RawResponse=@Raw,
                    ThoiGianThanhToan=CASE WHEN @TrangThai=1 THEN SYSDATETIME() ELSE ThoiGianThanhToan END
                WHERE ThanhToanID=@id";

            Add(cmd, "@TrangThai", trangThai);
            Add(cmd, "@Raw", rawResponse);
            Add(cmd, "@id", thanhToanId);

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

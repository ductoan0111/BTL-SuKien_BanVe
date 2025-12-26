using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Events_Management.Repositories
{
    public class DonHangRepository:IDonHangRepository
    {
        public int InsertDonHang(DonHang dh, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"
                INSERT INTO dbo.DonHang(NguoiMuaID, SuKienID, TongTien, TrangThai)
                VALUES(@NguoiMuaID, @SuKienID, 0, 0);
                SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran);

            cmd.Parameters.AddWithValue("@NguoiMuaID", dh.NguoiMuaID);
            cmd.Parameters.AddWithValue("@SuKienID", dh.SuKienID);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int UpdateTongTien(int donHangId, decimal tongTien, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(
                @"UPDATE dbo.DonHang SET TongTien=@t WHERE DonHangID=@id;", conn, tran);

            cmd.Parameters.AddWithValue("@t", tongTien);
            cmd.Parameters.AddWithValue("@id", donHangId);

            return cmd.ExecuteNonQuery();
        }

        public int InsertChiTiet(ChiTietDonHang ct, SqlConnection conn, SqlTransaction tran)
        {
            using var cmd = new SqlCommand(@"
                INSERT INTO dbo.ChiTietDonHang(DonHangID, LoaiVeID, SoLuong, DonGia)
                VALUES(@DonHangID, @LoaiVeID, @SoLuong, @DonGia);
                SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran);

            cmd.Parameters.AddWithValue("@DonHangID", ct.DonHangID);
            cmd.Parameters.AddWithValue("@LoaiVeID", ct.LoaiVeID);
            cmd.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
            cmd.Parameters.AddWithValue("@DonGia", ct.DonGia);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public DonHang? GetDonHangById(int id)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT DonHangID, NguoiMuaID, SuKienID, NgayDat, TongTien, TrangThai
                FROM dbo.DonHang
                WHERE DonHangID=@id;", new SqlParameter("@id", id));

            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];

            return new DonHang
            {
                DonHangID = Convert.ToInt32(r["DonHangID"]),
                NguoiMuaID = Convert.ToInt32(r["NguoiMuaID"]),
                SuKienID = Convert.ToInt32(r["SuKienID"]),
                NgayDat = Convert.ToDateTime(r["NgayDat"]),
                TongTien = Convert.ToDecimal(r["TongTien"]),
                TrangThai = Convert.ToByte(r["TrangThai"])
            };
        }

        public List<ChiTietDonHang> GetChiTietByDonHangId(int donHangId)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT ChiTietID, DonHangID, LoaiVeID, SoLuong, DonGia
                FROM dbo.ChiTietDonHang
                WHERE DonHangID=@id
                ORDER BY ChiTietID;", new SqlParameter("@id", donHangId));

            var list = new List<ChiTietDonHang>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ChiTietDonHang
                {
                    ChiTietID = Convert.ToInt32(r["ChiTietID"]),
                    DonHangID = Convert.ToInt32(r["DonHangID"]),
                    LoaiVeID = Convert.ToInt32(r["LoaiVeID"]),
                    SoLuong = Convert.ToInt32(r["SoLuong"]),
                    DonGia = Convert.ToDecimal(r["DonGia"])
                });
            }
            return list;
        }

        public List<DonHang> GetByNguoiMuaId(int nguoiMuaId)
        {
            var dt = DataHelper.GetDataTable(@"
                SELECT DonHangID, NguoiMuaID, SuKienID, NgayDat, TongTien, TrangThai
                FROM dbo.DonHang
                WHERE NguoiMuaID=@id
                ORDER BY DonHangID DESC;", new SqlParameter("@id", nguoiMuaId));

            var list = new List<DonHang>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new DonHang
                {
                    DonHangID = Convert.ToInt32(r["DonHangID"]),
                    NguoiMuaID = Convert.ToInt32(r["NguoiMuaID"]),
                    SuKienID = Convert.ToInt32(r["SuKienID"]),
                    NgayDat = Convert.ToDateTime(r["NgayDat"]),
                    TongTien = Convert.ToDecimal(r["TongTien"]),
                    TrangThai = Convert.ToByte(r["TrangThai"])
                });
            }
            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Events_Management.Data;
using Events_Management.Models;
using Events_Management.Repositories.Interfaces;
namespace Events_Management.Repositories
{
    public class SuKienRepository : ISuKienRepository
    {
        public List<SuKien> GetAll()
        {
            string sql = @"SELECT SuKienID, TenSuKien, MoTa,
                                  ThoiGianBatDau, ThoiGianKetThuc,
                                  DiaDiemID, ToChucID, TrangThai
                           FROM SuKien";

            var dt = DataHelper.GetDataTable(sql);
            var list = new List<SuKien>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SuKien
                {
                    SuKienID = Convert.ToInt32(row["SuKienID"]),
                    TenSuKien = row["TenSuKien"].ToString() ?? "",
                    MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"].ToString(),
                    ThoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]),
                    ThoiGianKetThuc = Convert.ToDateTime(row["ThoiGianKetThuc"]),
                    DiaDiemID = Convert.ToInt32(row["DiaDiemID"]),
                    ToChucID = Convert.ToInt32(row["ToChucID"]),
                    TrangThai = Convert.ToBoolean(row["TrangThai"])
                });
            }

            return list;
        }

        public SuKien? GetById(int id)
        {
            string sql = @"SELECT SuKienID, TenSuKien, MoTa,
                                  ThoiGianBatDau, ThoiGianKetThuc,
                                  DiaDiemID, ToChucID, TrangThai
                           FROM SuKien
                           WHERE SuKienID = @id";

            var dt = DataHelper.GetDataTable(sql, new SqlParameter("@id", id));

            if (dt.Rows.Count == 0) return null;

            var row = dt.Rows[0];

            return new SuKien
            {
                SuKienID = Convert.ToInt32(row["SuKienID"]),
                TenSuKien = row["TenSuKien"].ToString() ?? "",
                MoTa = row["MoTa"] == DBNull.Value ? null : row["MoTa"].ToString(),
                ThoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]),
                ThoiGianKetThuc = Convert.ToDateTime(row["ThoiGianKetThuc"]),
                DiaDiemID = Convert.ToInt32(row["DiaDiemID"]),
                ToChucID = Convert.ToInt32(row["ToChucID"]),
                TrangThai = Convert.ToBoolean(row["TrangThai"])
            };
        }

        public int Insert(SuKien sk)
        {
            string sql = @"INSERT INTO SuKien
                           (TenSuKien, MoTa, ThoiGianBatDau, ThoiGianKetThuc,
                            DiaDiemID, ToChucID, TrangThai)
                           VALUES
                           (@TenSuKien, @MoTa, @ThoiGianBatDau, @ThoiGianKetThuc,
                            @DiaDiemID, @ToChucID, @TrangThai)";

            var p = new[]
            {
                new SqlParameter("@TenSuKien", sk.TenSuKien),
                new SqlParameter("@MoTa", (object?)sk.MoTa ?? DBNull.Value),
                new SqlParameter("@ThoiGianBatDau", sk.ThoiGianBatDau),
                new SqlParameter("@ThoiGianKetThuc", sk.ThoiGianKetThuc),
                new SqlParameter("@DiaDiemID", sk.DiaDiemID),
                new SqlParameter("@ToChucID", sk.ToChucID),
                new SqlParameter("@TrangThai", sk.TrangThai),
            };

            return DataHelper.ExecuteNonQuery(sql, p);
        }

        public int Update(SuKien sk)
        {
            string sql = @"UPDATE SuKien
                           SET TenSuKien = @TenSuKien,
                               MoTa = @MoTa,
                               ThoiGianBatDau = @ThoiGianBatDau,
                               ThoiGianKetThuc = @ThoiGianKetThuc,
                               DiaDiemID = @DiaDiemID,
                               ToChucID = @ToChucID,
                               TrangThai = @TrangThai
                           WHERE SuKienID = @SuKienID";

            var p = new[]
            {
                new SqlParameter("@TenSuKien", sk.TenSuKien),
                new SqlParameter("@MoTa", (object?)sk.MoTa ?? DBNull.Value),
                new SqlParameter("@ThoiGianBatDau", sk.ThoiGianBatDau),
                new SqlParameter("@ThoiGianKetThuc", sk.ThoiGianKetThuc),
                new SqlParameter("@DiaDiemID", sk.DiaDiemID),
                new SqlParameter("@ToChucID", sk.ToChucID),
                new SqlParameter("@TrangThai", sk.TrangThai),
                new SqlParameter("@SuKienID", sk.SuKienID),
            };

            return DataHelper.ExecuteNonQuery(sql, p);
        }
    }
}

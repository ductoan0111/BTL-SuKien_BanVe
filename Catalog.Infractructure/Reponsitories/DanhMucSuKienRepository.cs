using BuildingBlocks.Shareds.Abstractions;
using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infractructure.Reponsitories
{
    public class DanhMucSuKienRepository : IDanhMucSuKienRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DanhMucSuKienRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<DanhMucSuKien> GetAll()
        {
            var list = new List<DanhMucSuKien>();
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM DanhMucSuKien WHERE TrangThai = 1";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public DanhMucSuKien? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM DanhMucSuKien WHERE DanhMucID = @id";
            AddParam(cmd, "@id", id);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Create(DanhMucSuKien entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO DanhMucSuKien (TenDanhMuc, MoTa, ThuTuHienThi, TrangThai)
                OUTPUT INSERTED.DanhMucID
                VALUES (@Ten, @MoTa, @ThuTu, @TrangThai)";

            AddParam(cmd, "@Ten", entity.TenDanhMuc);
            AddParam(cmd, "@MoTa", entity.MoTa);
            AddParam(cmd, "@ThuTu", entity.ThuTuHienThi);
            AddParam(cmd, "@TrangThai", entity.TrangThai);

            return (int)cmd.ExecuteScalar()!;
        }

        public bool Update(DanhMucSuKien entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE DanhMucSuKien
                SET TenDanhMuc=@Ten, MoTa=@MoTa, ThuTuHienThi=@ThuTu, TrangThai=@TrangThai
                WHERE DanhMucID=@Id";

            AddParam(cmd, "@Id", entity.DanhMucID);
            AddParam(cmd, "@Ten", entity.TenDanhMuc);
            AddParam(cmd, "@MoTa", entity.MoTa);
            AddParam(cmd, "@ThuTu", entity.ThuTuHienThi);
            AddParam(cmd, "@TrangThai", entity.TrangThai);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE DanhMucSuKien SET TrangThai = 0 WHERE DanhMucID = @id";
            AddParam(cmd, "@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private DanhMucSuKien Map(IDataReader r)
        {
            return new DanhMucSuKien
            {
                DanhMucID = (int)r["DanhMucID"],
                TenDanhMuc = r["TenDanhMuc"].ToString()!,
                MoTa = r["MoTa"] as string,
                ThuTuHienThi = r["ThuTuHienThi"] as int?,
                TrangThai = (bool)r["TrangThai"]
            };
        }

        private void AddParam(IDbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}

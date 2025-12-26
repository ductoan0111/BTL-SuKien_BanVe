using BuildingBlocks.Shareds.Abstractions;
using Catalog.Application.Contracts.Reponsitories;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Reponsitories
{
    public class SuKienToChucRepository : ISuKienToChucRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SuKienToChucRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<SuKienToChuc> GetBySuKien(int suKienId)
        {
            var list = new List<SuKienToChuc>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT SuKienId, NguoiDungId, VaiTroTrongSuKien, NgayThamGia
            FROM SuKienToChuc
            WHERE SuKienId = @SuKienId
            ORDER BY NgayThamGia DESC;";

            AddParam(cmd, "@SuKienId", suKienId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public SuKienToChuc? Get(int suKienId, int nguoiDungId)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT SuKienId, NguoiDungId, VaiTroTrongSuKien, NgayThamGia
            FROM SuKienToChuc
            WHERE SuKienId = @SuKienId AND NguoiDungId = @NguoiDungId;";

            AddParam(cmd, "@SuKienId", suKienId);
            AddParam(cmd, "@NguoiDungId", nguoiDungId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }
        public bool Create(SuKienToChuc entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO SuKienToChuc
                (SuKienId, NguoiDungId, VaiTroTrongSuKien)
            VALUES
                (@SuKienId, @NguoiDungId, @VaiTro);";

            AddParam(cmd, "@SuKienId", entity.SuKienId);
            AddParam(cmd, "@NguoiDungId", entity.NguoiDungId);
            AddParam(cmd, "@VaiTro", entity.VaiTroTrongSuKien);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Update(SuKienToChuc entity)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE SuKienToChuc
            SET VaiTroTrongSuKien = @VaiTro
            WHERE SuKienId = @SuKienId AND NguoiDungId = @NguoiDungId;";

            AddParam(cmd, "@VaiTro", entity.VaiTroTrongSuKien);
            AddParam(cmd, "@SuKienId", entity.SuKienId);
            AddParam(cmd, "@NguoiDungId", entity.NguoiDungId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int suKienId, int nguoiDungId)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            DELETE FROM SuKienToChuc
            WHERE SuKienId = @SuKienId AND NguoiDungId = @NguoiDungId;";

            AddParam(cmd, "@SuKienId", suKienId);
            AddParam(cmd, "@NguoiDungId", nguoiDungId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public List<SuKienToChuc> GetAll()
        {
            var list = new List<SuKienToChuc>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        SELECT SuKienId, NguoiDungId, VaiTroTrongSuKien, NgayThamGia
        FROM SuKienToChuc
        ORDER BY NgayThamGia DESC;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        private static void AddParam(IDbCommand cmd, string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static SuKienToChuc Map(IDataRecord r)
        {
            return new SuKienToChuc
            {
                SuKienId = Convert.ToInt32(r["SuKienId"]),
                NguoiDungId = Convert.ToInt32(r["NguoiDungId"]),
                VaiTroTrongSuKien = Convert.ToString(r["VaiTroTrongSuKien"])!,
                NgayThamGia = Convert.ToDateTime(r["NgayThamGia"])
            };
        }
    }
}

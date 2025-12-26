using System.Data;
using BuildingBlocks.Shareds.Abstractions;
using Identity.Application.Contracts.Repositories;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Repositories
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public VaiTroRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<VaiTro> GetAll()
        {
            var list = new List<VaiTro>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT VaiTroID, MaVaiTro, TenVaiTro, NgayTao
            FROM VaiTro";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public VaiTro? GetById(int id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT VaiTroID, MaVaiTro, TenVaiTro, NgayTao
                FROM VaiTro
                WHERE VaiTroID = @Id";

            AddParam(cmd, "@Id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return Map(reader);
        }

        private static void AddParam(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static VaiTro Map(IDataRecord r)
        {
            return new VaiTro
            {
                VaiTroID = r.GetInt32(r.GetOrdinal("VaiTroID")),
                MaVaiTro = r.GetString(r.GetOrdinal("MaVaiTro")),
                TenVaiTro = r.GetString(r.GetOrdinal("TenVaiTro")),
                NgayTao = r.GetDateTime(r.GetOrdinal("NgayTao"))
            };
        }
    }
}

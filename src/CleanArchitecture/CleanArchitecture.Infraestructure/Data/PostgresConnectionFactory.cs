using CleanArchitecture.Application.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Data
{
    internal sealed class PostgresConnectionFactory : IPostgresConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            // como trabajamos con PostGre, se devuelve un tipo de conexion Postgre
            var connection = new Npgsql.NpgsqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}

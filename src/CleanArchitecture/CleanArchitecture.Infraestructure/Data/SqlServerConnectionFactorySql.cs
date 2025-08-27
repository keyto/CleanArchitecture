using CleanArchitecture.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infraestructure.Data
{
    internal class SqlServerConnectionFactorySql : ISqlServerConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactorySql(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            // como trabajamos con PostGre, se devuelve un tipo de conexion Postgre
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}

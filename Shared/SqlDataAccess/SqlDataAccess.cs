using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Strona_v2.Shared.SqlDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionStringName { get; set; } = "Default";

        public string GetConnectionString()
        {
            return _config.GetConnectionString(ConnectionStringName);
        }


        public async Task<List<T>> LoadDataList<T>(string sql)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql);
                return data.ToList();
            }
        }

        public async Task<T> LoadData<T>(string sql)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryFirstOrDefaultAsync<T>(sql);
                return (T)data;
            }
        }



        public async Task SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}

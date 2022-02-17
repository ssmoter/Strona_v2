using Strona_v2.Shared.SqlDataAccess;
using System.Data.SqlClient;


namespace Strona_v2.Server.Data.SqlCreatTable
{
    public interface ICreatTable
    {
        Task<bool> CreatNewSQLTask(string Comment);
    }

    public class CreatTable : ICreatTable
    {
        private readonly string ConnectionString;
        private readonly ISqlDataAccess _sqlData;
        private SqlConnection _connection;
        private SqlCommand? _command;


        public CreatTable(ISqlDataAccess sqlData)
        {
            _sqlData = sqlData;
            ConnectionString = _sqlData.GetConnectionString();
            _connection = new SqlConnection(ConnectionString);
        }

        //tworzenie połączenia i wykonanie komendy (stworzenie/usunięcie bazy danych (taki plan))
        public async Task<bool> CreatNewSQLTask(string Comment)
        {
            bool result = false;
            try
            {
                _command = new SqlCommand(Comment, _connection);
                _connection.Open();
                await _command.ExecuteNonQueryAsync();
                _connection.Close();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                return result;
            }
        }


    }
}

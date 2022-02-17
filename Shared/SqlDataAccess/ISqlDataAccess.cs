
namespace Strona_v2.Shared.SqlDataAccess
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        string GetConnectionString();
        Task<T> LoadData<T>(string sql);
        Task<List<T>> LoadDataList<T>(string sql);
        Task SaveData<T>(string sql, T parameters);
    }
}
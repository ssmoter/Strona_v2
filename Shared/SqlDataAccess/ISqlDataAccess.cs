
namespace Strona_v2.Shared.SqlDataAccess
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<T> LoadData<T>(string sql);
        Task<List<T>> LoadDataList<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}
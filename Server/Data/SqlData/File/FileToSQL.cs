using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;
using System.Globalization;

namespace Strona_v2.Server.Data.SqlData.File
{
    public interface IFileToSQL
    {
        Task CreateFile(FileModelServer file);
        Task DeteteFileModel(int Id);
        Task<FileModelServer> GetFileModel(int file);
        Task<IList<FileModelServer>> GetFileModels();
        Task<IList<FileModelServer>> GetFileModelsSimple();
        Task UpdateFile(FileModelServer file);
        Task<int> GetFileModelFromDate(DateTimeOffset? date);
        Task<FileModelServer> GetFileModelsSimpleOne(int id);
    }

    public class FileToSQL : IFileToSQL
    {
        private readonly ISqlDataAccess _SqlDataAccess;
        private readonly string TableName = nameof(FileModelC);
        public FileToSQL(ISqlDataAccess sqlDataAccess)
        {
            _SqlDataAccess = sqlDataAccess;
        }

        //zapisywanie danych pod nowy szereg
        public async Task CreateFile(FileModelServer file)
        {
            string sql = "INSERT INTO dbo." + TableName +
                " \n(" + nameof(file.Title) + ", " + nameof(file.Description) +
                ", " + nameof(file.UserId) + ", " + nameof(file.Created) +
                 ", " + nameof(file.StoredFileName) + ", " + nameof(file.Type) +
                ") \nVALUES \n(@" +
                nameof(file.Title) + ", @" + nameof(file.Description) +
                ", @" + nameof(file.UserId) + ", @" + nameof(file.Created) + ", @" +
                 nameof(file.StoredFileName) + ", @" + nameof(file.Type) + ")";

            await _SqlDataAccess.SaveData(sql, file);

        }

        //dopisanie nowego pliku do istniejącego szeregu 
        public async Task UpdateFile(FileModelServer file)
        {
            string sql = "UPDATE dbo." + TableName +
                " SET " + nameof(file.StoredFileName) + " = @" + nameof(file.StoredFileName) +
                "," + nameof(file.Type) + " = @" + nameof(file.Type) +
                "WHERE " + nameof(file.Id) + " = " + file.Id;

            await _SqlDataAccess.SaveData(sql, file);
        }

        //pobranie jednego obiektu
        public async Task<FileModelServer> GetFileModel(int file)
        {
            string sql = "SELECT * " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(FileModelServer.Id) + " = " + file;

            return await _SqlDataAccess.LoadData<FileModelServer>(sql);
        }

        //pobranie listy
        public async Task<IList<FileModelServer>> GetFileModels()
        {
            string sql = "SELECT * " +
                   " FROM dbo." + TableName;

            return await _SqlDataAccess.LoadDataList<FileModelServer>(sql);
        }

        //pobieranie Id i daty
        public async Task<IList<FileModelServer>> GetFileModelsSimple()
        {
            string sql = "SELECT " +
                nameof(FileModelServer.Id) + "," + nameof(FileModelServer.Created) +
                "," + nameof(FileModelServer.NoLike) + "," + nameof(FileModelServer.UnLike) +
                " FROM " + TableName;

            return await _SqlDataAccess.LoadDataList<FileModelServer>(sql);
        }
        public async Task<FileModelServer> GetFileModelsSimpleOne(int id)
        {
            string sql = "SELECT " +
                nameof(FileModelServer.Id) + "," + nameof(FileModelServer.Created) +
                "," + nameof(FileModelServer.NoLike) + "," + nameof(FileModelServer.UnLike) +
                " FROM " + TableName +
                " WHERE " + nameof(FileModelServer.Id) + " = " + id;

            return await _SqlDataAccess.LoadData<FileModelServer>(sql);
        }


        //Usuwanie rekordu
        public async Task DeteteFileModel(int Id)
        {
            string sql = "DELETE FROM dbo." + TableName +
                " WHERE " + nameof(FileModelServer.Id) + " = " + Id;

            await _SqlDataAccess.SaveData(sql, Id);
        }

        public Task<int> GetFileModelFromDate(DateTimeOffset? Created)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            string sql = "SELECT Id " +
                "FROM dbo." + TableName +
                " WHERE " + nameof(Created) + "='" + Created.Value.ToString("yyyy-MM-dd HH:mm:ss.fffffff K") + "'";

            return _SqlDataAccess.LoadData<int>(sql);
        }

    }
}

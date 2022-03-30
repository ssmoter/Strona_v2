using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.SqlData.File
{
    public interface IFileToSQL
    {
        Task CreateFile(FileModelServer file);
        Task<FileModelServer> GetFileModel(FileModelServer file);
        Task<IList<FileModelServer>> GetFileModels();
        Task UpdateFile(FileModelServer file);
    }

    public class FileToSQL : IFileToSQL
    {
        private readonly ISqlDataAccess _SqlDataAccess;
        private readonly string TableName = "FileData";
        public FileToSQL(ISqlDataAccess sqlDataAccess)
        {
            _SqlDataAccess = sqlDataAccess;
        }

        //zapisywanie danych pod nowy szereg
        public async Task CreateFile(FileModelServer file)
        {
            string sql = "INSERT INTO dbo." + TableName +
                " \n(" + nameof(file.Title) + ", " + nameof(file.Description) + ", " + nameof(file.Tag) +
                ", " + nameof(file.UserId) + ", " + nameof(file.Created)  +
                 ", " + nameof(file.StoredFileName) + ", " + nameof(file.Type) +
                ") \nVALUES \n(@" +
                nameof(file.Title) + ", @" + nameof(file.Description) + ", @" + nameof(file.Tag) +
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



            string sq = string.Format("UPDATE dbo.{tableName} " +
                    " SET {FileName} = @{fileName}, {Type} = @{type} " +
                    " WHERE {Id} = {id}",
                    TableName, nameof(file.StoredFileName), file.StoredFileName,
                    nameof(file.Type), file.Type,
                    nameof(file.Id), file.Id);

            await _SqlDataAccess.SaveData(sql, file);
        }

        //pobranie jednego obiektu
        public async Task<FileModelServer> GetFileModel(FileModelServer file)
        {
            string sql = "SELECT * " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(file.Id) + " = " + file.Id;


            return await _SqlDataAccess.LoadData<FileModelServer>(sql);
        }

        //pobranie listy
        public async Task<IList<FileModelServer>> GetFileModels()
        {
            string sql = "SELECT * " +
                   " FROM dbo." + TableName;

            return await _SqlDataAccess.LoadDataList<FileModelServer>(sql);
        }

    }
}

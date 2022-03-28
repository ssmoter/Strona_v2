using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.SqlData.File
{
    public interface IFileToSQL
    {
        Task CreateFile(FileModel file);
        Task<FileModel> GetFileModel(FileModel file);
        Task<IList<FileModel>> GetFileModels();
        Task UpdateFile(FileModel file);
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
        public async Task CreateFile(FileModel file)
        {
            string sql = "INSERT INTO dbo." + TableName +
                " \n(" + nameof(file.Title) + ", " + nameof(file.Description) + ", " + nameof(file.Tag) +
                ", " + nameof(file.UserId) + ", " + nameof(file.Created) + ", " + nameof(file.Path) +
                 ", " + nameof(file.StoredFileName) + ", " + nameof(file.Type) +
                ") \nVALUES \n(@" +
                nameof(file.Title) + ", @" + nameof(file.Description) + ", @" + nameof(file.Tag) +
                ", @" + nameof(file.UserId) + ", @" + nameof(file.Created) + ", @" + nameof(file.Path) + ", @" +
                 nameof(file.StoredFileName) + ", @" + nameof(file.Type) + ")";

            string sq = "INSERT INTO dbo.FileData "+
               " (Title, Description, Tag, UserId, Created, Path, StoredFileName, Type)"+
               " VALUES"+
               " (@Title, @Description, @Tag, @UserId, @Created, @Path, @StoredFileName, @Type)";


            await _SqlDataAccess.SaveData(sql, file);

        }

        //dopisanie nowego pliku do istniejącego szeregu 
        public async Task UpdateFile(FileModel file)
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
        public async Task<FileModel> GetFileModel(FileModel file)
        {
            string sql = "SELECT * " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(file.Id) + " = " + file.Id;


            return await _SqlDataAccess.LoadData<FileModel>(sql);
        }

        //pobranie listy
        public async Task<IList<FileModel>> GetFileModels()
        {
            string sql = "SELECT * " +
                   " FROM dbo." + TableName;

            return await _SqlDataAccess.LoadDataList<FileModel>(sql);
        }

    }
}

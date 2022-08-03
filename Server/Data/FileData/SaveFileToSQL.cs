﻿using Strona_v2.Server.Data.SqlCreatTable;
using Strona_v2.Server.Data.SqlData.File;
using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.FileData
{
    public interface ISaveFileToSQL
    {
        Task<bool> SaveAsync(FileModelServer fileModel, ILogger logger);
        Task<bool> SaveAsyncReapit(FileModelServer fileModel, ILogger logger);
    }

    public class SaveFileToSQL : ISaveFileToSQL
    {

        private readonly ISqlDataAccess _sqlDataAccess;
        private FileToSQL fileToSQL;

        public SaveFileToSQL(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
            fileToSQL = new(sqlDataAccess);
        }


        //Zapisanie danych do bazy danych i utworzenie nowej tabeli
        public async Task<bool> SaveAsync(FileModelServer fileModel, ILogger logger)
        {
            try
            {
                //zapisanie danych do sql
                await fileToSQL.CreateFile(fileModel);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error:" + ex.Message);
                return false;
            }
        }

        //Przy błędzie możliwość ponownego dopisania obrazów do tego samego rekordu
        public async Task<bool> SaveAsyncReapit(FileModelServer fileModel, ILogger logger)
        {
            bool result = false;

            FileModelServer file = await fileToSQL.GetFileModel(fileModel.Id);

            if (file.Created.Value.Year == DateTimeOffset.Now.Year && file.Created.Value.Day == DateTimeOffset.Now.Day)
            {
                if (file.Created.Value.AddMinutes(15).Minute > DateTimeOffset.Now.Minute)
                {
                    if (fileModel.Files != null)
                    {

                        fileModel.StoredFileName = CombineToOneReapit(fileModel, file, 1);
                        fileModel.Type = CombineToOneReapit(fileModel, file, 2);
                    }
                    try
                    {
                        await fileToSQL.UpdateFile(file);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        logger.LogInformation("Error:" + ex.Message);
                        throw;
                    }
                }
            }


            return result;
        }

        //połączenie listy nazw w jeden string ; nr = 1
        //połączenie listy typów w jeden string ;nr = 2
        private string? CombineToOneReapit(FileModelServer fileModel, FileModelServer file, int n)
        {
            string result = string.Empty;

            switch (n)
            {
                case 1:
                    for (int i = 0; i < fileModel.Files.Count; i++)
                    {
                        if (fileModel.Files[i].ErrorCode != 0)
                        {
                            file.StoredFileName += fileModel.Files[i].StoredFileName;
                        }
                    }
                    result = file.StoredFileName;
                    return result;
                case 2:
                    for (int i = 0; i < fileModel.Files.Count; i++)
                    {
                        if (fileModel.Files[i].ErrorCode != 0)
                        {
                            file.Type += fileModel.Files[i].Type;
                        }
                    }
                    result = file.Type;
                    return result;
                default:
                    return result;
            }
        }


        //utworzenie nowej tabeli dla komentarzy 
        private async Task<bool> CreatedTabelForComment(int FileId)
        {
            ExampleTable exampleTable = new(6);
            CommentModelServer commentModel = new CommentModelServer();


            exampleTable.ColumnName[0] = nameof(commentModel.FileId);
            exampleTable.ColumnParametr[0] = "int";

            exampleTable.ColumnName[1] = nameof(commentModel.UserId);
            exampleTable.ColumnParametr[1] = "int";

            exampleTable.ColumnName[2] = nameof(commentModel.Comment);
            exampleTable.ColumnParametr[2] = "nvarchar(MAX)";

            exampleTable.ColumnName[5] = nameof(commentModel.Created);
            exampleTable.ColumnParametr[5] = "datetimeoffset";

            string Question = exampleTable.CreatSqlTable(nameof(CommentModelServer), FileId.ToString());

            CreatTable creatTable = new CreatTable(_sqlDataAccess);

            bool result = await creatTable.CreatNewSQLTask(Question);
            return result;
        }


        //utworzenie nowej tabeli dla like/unlike
        private async Task<bool> CreatedTabelForLike(int FileId)
        {
            ExampleTable exampleTable = new(8);
            CommentModelServer commentModel = new CommentModelServer();


            string Question = exampleTable.CreatSqlTable(nameof(CommentModelServer), FileId.ToString());

            CreatTable creatTable = new CreatTable(_sqlDataAccess);

            bool result = await creatTable.CreatNewSQLTask(Question);
            return result;
        }

    }
}

using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.CommentData
{
    public interface ISaveCommentToSQL
    {
        Task<IList<CommentModelServer>> GetCommentsAsync(int FileId);
        Task SetCommentsAsync(CommentModelServer comment);
    }

    public class SaveCommentToSQL : ISaveCommentToSQL
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        private readonly string TableName = nameof(CommentModelServer) + "_";
        public SaveCommentToSQL(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        //pobieranie komentarzy z jednego posta
        public async Task<IList<CommentModelServer>> GetCommentsAsync(int FileId)
        {
            string sql = "SELECT * FROM dbo." + TableName + FileId.ToString();

            // sql = "SELECT * FROM dbo.CommentModelServer_4002";
            return await _sqlDataAccess.LoadDataList<CommentModelServer>(sql);
        }

        //zapisywanie komentarzy
        public async Task SetCommentsAsync(CommentModelServer comment)
        {
            string sql = "INSERT INTO dbo." + TableName + comment.FileId +
                          " \n(" + nameof(CommentModelServer.FileId) + ", " + nameof(CommentModelServer.UserId) +
                          ", " + nameof(CommentModelServer.Comment) + ", " + nameof(CommentModelServer.NoLike) +
                          ", " + nameof(CommentModelServer.UnLike) + ", " + nameof(CommentModelServer.Created) +
                           ", " + nameof(CommentModelServer.ListUserIdLike) + ", " + nameof(CommentModelServer.ListUserIdUnLike) +
                          ")\n VALUES\n(@"
                               + nameof(CommentModelServer.FileId) + ", @" + nameof(CommentModelServer.UserId) +
                          ", @" + nameof(CommentModelServer.Comment) + ", @" + nameof(CommentModelServer.NoLike) +
                          ", @" + nameof(CommentModelServer.UnLike) + ", @" + nameof(CommentModelServer.Created) +
                          ", @" + nameof(CommentModelServer.ListUserIdLike) + ", @" + nameof(CommentModelServer.ListUserIdUnLike) + ")";
            await _sqlDataAccess.SaveData(sql, comment);
        }


    }
}

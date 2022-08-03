using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.TagData
{
    public interface ITagSql
    {
        Task<List<int>> GetFileIdFromTags(string tag);
        Task<List<TagModelServer>> GetTagsList(int id);
        Task SaveTag(TagModelServer server);

    }

    public class TagSql : ITagSql
    {
        private readonly ISqlDataAccess _SqlDataAccess;
        private readonly string TableName = nameof(TagModel);

        public TagSql(ISqlDataAccess sqlDataAccess)
        {
            _SqlDataAccess = sqlDataAccess;
        }

        //zapisywanie tagu
        public async Task SaveTag(TagModelServer server)
        {
            string sql = "INSERT INTO dbo." + TableName +
                " \n(" + nameof(server.FileId) + ", " + nameof(server.UserId) +
                ", " + nameof(server.Value) +
                ") \nVALUES \n(@" +
                nameof(server.FileId) + ", @" + nameof(server.UserId) +
                ", @" + nameof(server.Value) + ")";

            await _SqlDataAccess.SaveData(sql, server);
        }

        public async Task<List<TagModelServer>> GetTagsList(int id)
        {
            string sql = "SELECT * " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(TagModelServer.FileId) + " = " + id;
            return await _SqlDataAccess.LoadDataList<TagModelServer>(sql);
        }

        public async Task<List<int>> GetFileIdFromTags(string tag)
        {
            string sql = "SELECT " + nameof(TagModelServer.FileId) +
                " FROM dbo." + TableName +
                " WHERE " + nameof(TagModel.Value) + " = N'" + tag + "'";

            return await _SqlDataAccess.LoadDataList<int>(sql);
        }

    }
}

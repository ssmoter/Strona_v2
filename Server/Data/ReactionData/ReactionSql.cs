using Strona_v2.Shared.File;
using Strona_v2.Shared.SqlDataAccess;

namespace Strona_v2.Server.Data.ReactionData
{
    public interface IReactionSql
    {
        Task<ReactionModelServer> GetConcreteUserReaction(ReactionModelServer server);
        Task InsertConcreteUserReaction(ReactionModelServer server);
        Task UpdateConcreteUserReaction(ReactionModelServer server);
    }

    public class ReactionSql : IReactionSql
    {
        private readonly ISqlDataAccess _SqlDataAccess;
        private readonly string TableName = nameof(ReactionModel);

        public ReactionSql(ISqlDataAccess sqlDataAccess)
        {
            _SqlDataAccess = sqlDataAccess;
        }

        //pobranie rekordu z (nie)polubionego obiektu usera
        public async Task<ReactionModelServer> GetConcreteUserReaction(ReactionModelServer server)
        {
            string sql = "SELECT * FROM dbo." + TableName +
                " WHERE " + nameof(ReactionModelServer.TypeObject) + " = " + (int)server.TypeObject +
                " AND " + nameof(ReactionModelServer.UserId) + " = " + server.UserId +
                " AND " + nameof(ReactionModelServer.ObjectId) + " = " + server.ObjectId;

            return await _SqlDataAccess.LoadData<ReactionModelServer>(sql);
        }

        //aktualizowanie zmiany przez usera
        public async Task UpdateConcreteUserReaction(ReactionModelServer server)
        {
            string sql = "UPDATE dbo." + TableName +
                " SET " + nameof(ReactionLevel) + " = " + (int)server.Level +
                " WHERE " + nameof(ReactionModelServer.TypeObject) + " = " + (int)server.TypeObject +
                " AND " + nameof(ReactionModelServer.UserId) + " = " + server.UserId +
                " AND " + nameof(ReactionModelServer.ObjectId) + " = " + server.ObjectId;

            await _SqlDataAccess.SaveData(sql, server);
        }

        //dodanie nowej reakcji
        public async Task InsertConcreteUserReaction(ReactionModelServer server)
        {
            string sql = "INSERT INTO dbo." + TableName +
                "\n(" + nameof(ReactionModelServer.ObjectId) + "," +
                nameof(ReactionModelServer.UserId) + "," +
                nameof(ReactionLevel) + "," +
                nameof(ReactionType) +
                ")\n VALUES\n( @" +
                nameof(ReactionModelServer.ObjectId) + ", @" +
                nameof(ReactionModelServer.UserId) + ", @" +
                nameof(ReactionLevel) + ", @" +
                nameof(ReactionType) + ")";


            await _SqlDataAccess.SaveData(sql, server);
        }
    }
}

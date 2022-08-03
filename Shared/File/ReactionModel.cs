using HashidsNet;

namespace Strona_v2.Shared.File
{

    public enum ReactionLevel
    {
        NoLike = 1,
        DontCare = 0,
        Unlike = -1,
    }

    public enum ReactionType
    {
        UserData=1,
        FileModelC=2,
        CommentModel=3,
    }
    public class ReactionModel
    {
        public ReactionLevel? Level { get; set; } = ReactionLevel.DontCare;
        public ReactionType? TypeObject { get; set; }
    }

    public class ReactionModelClient : ReactionModel
    {
        public string? Id { get; set; }
        public string? ObjectId { get; set; }
        public string? UserId { get; set; }
        public ReactionModelClient()
        { }
        public ReactionModelClient(ReactionModelServer server, IHashids hashids)
        {
            Id = hashids.Encode(server.Id);
            ObjectId = hashids.Encode(server.ObjectId);
            UserId = hashids.Encode(server.UserId);
            Level = server.Level;
            TypeObject = server.TypeObject;
        }
    }
    public class ReactionModelServer : ReactionModel
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int UserId { get; set; }

        public ReactionModelServer()
        { }
        public ReactionModelServer(ReactionModelClient client, IHashids hashids)
        {
            if (!string.IsNullOrEmpty(client.Id))
            {
                Id = hashids.Decode(client.Id)[0];
            }
            if (!string.IsNullOrEmpty(client.ObjectId))
            {
                ObjectId = hashids.Decode(client.ObjectId)[0];
            }
            if (!string.IsNullOrEmpty(client.UserId))
            {
                UserId = hashids.Decode(client.UserId)[0];
            }
            Level = client.Level;
            TypeObject = client.TypeObject;
        }

    }
}

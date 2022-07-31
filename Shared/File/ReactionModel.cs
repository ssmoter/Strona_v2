using HashidsNet;

namespace Strona_v2.Shared.File
{

    public enum ReactionLevel
    {
        NoLike = 1,
        DontCare = 0,
        Unlike = -1,
    }
    public class ReactionModel
    {
        public ReactionLevel? Level { get; set; }
    }

    public class ReactionModelClient : ReactionModel
    {
        public string? Id { get; set; }
        public string? FileId { get; set; }
        public string? UserId { get; set; }
        public ReactionModelClient()
        { }
        public ReactionModelClient(ReactionModelServer server, IHashids hashids)
        {
            Id = hashids.Encode(server.Id);
            FileId = hashids.Encode(server.FileId);
            UserId = hashids.Encode(server.UserId);
            Level = server.Level;
        }
    }
    public class ReactionModelServer : ReactionModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }

        public ReactionModelServer()
        { }
        public ReactionModelServer(ReactionModelClient client, IHashids hashids)
        {
            Id = hashids.Decode(client.Id)[0];
            FileId = hashids.Decode(client.FileId)[0];
            UserId = hashids.Decode(client.UserId)[0];
            Level = client.Level;
        }

    }
}

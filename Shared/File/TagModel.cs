using HashidsNet;

namespace Strona_v2.Shared.File
{
    public class TagModel
    {
        public string? Value { get; set; }

    }

    public class TagModelClient : TagModel
    {
        public string? Id { get; set; }
        public string? FileId { get; set; }
        public string? UserId { get; set; }
        public DateTimeOffset? Created { get; set; }
        public TagModelClient()
        { }
        public TagModelClient(TagModelServer server, IHashids hashids)
        {
            Id = hashids.Encode(server.Id);
            FileId = hashids.Encode(server.FileId);
            UserId = hashids.Encode(server.UserId);
            Value = server.Value;
        }
    }

    public class TagModelServer : TagModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public TagModelServer()
        { }
        public TagModelServer(TagModelClient client, IHashids hashids)
        {
            if (!string.IsNullOrEmpty(client.Id))
                Id = hashids.Decode(client.Id)[0];

            if (!string.IsNullOrEmpty(client.FileId))
                FileId = hashids.Decode(client.FileId)[0];

            if (!string.IsNullOrEmpty(client.UserId))
                UserId = hashids.Decode(client.UserId)[0];

            Value = client.Value;
        }
        public TagModelServer(TagModelClient client, int fileId, IHashids hashids)
        {
            if (!string.IsNullOrEmpty(client.Id))
                Id = hashids.Decode(client.Id)[0];

            FileId = fileId;

            if (!string.IsNullOrEmpty(client.UserId))
                UserId = hashids.Decode(client.UserId)[0];

            Value = client.Value;
        }
    }
}

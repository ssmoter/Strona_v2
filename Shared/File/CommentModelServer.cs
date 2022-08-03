using HashidsNet;

namespace Strona_v2.Shared.File
{
    public class CommentModel
    {
        public string? Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public int NoLike { get; set; }
        public int UnLike { get; set; }

    }

    public class CommentModelClient : CommentModel
    {
        public string? Id { get; set; }
        public string? FileId { get; set; }
        public string? UserId { get; set; }
        public CommentModelClient()
        { }
        public CommentModelClient(CommentModelServer server, IHashids hashids)
        {
            Id = hashids.Encode(server.Id);
            FileId = hashids.Encode(server.FileId);
            UserId = hashids.Encode(server.UserId);
            Comment = server.Comment;
            Created = server.Created;
        }
    }
    public class CommentModelServer : CommentModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }

        public CommentModelServer()
        { }
        public CommentModelServer(CommentModelClient client, IHashids hashids)
        {
            if (!string.IsNullOrEmpty(client.Id))
            {
                var n = hashids.Decode(client.Id);
                Id = n[0];
            }
            if (!string.IsNullOrEmpty(client.FileId))
            {
                var n = hashids.Decode(client.FileId);
                FileId = n[0];
            }
            if (!string.IsNullOrEmpty(client.UserId))
            {
                var n = hashids.Decode(client.UserId);
                UserId = n[0];
            }
            Comment = client.Comment;
            Created = client.Created;
        }
    }

}

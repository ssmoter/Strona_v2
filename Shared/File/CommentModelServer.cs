namespace Strona_v2.Shared.File
{
    public class CommentModelServer
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string? Comment { get; set; }
        public int NoLike { get; set; }
        public int UnLike { get; set; }
        public DateTimeOffset Created { get; set; }

        public string? ListUserIdLike { get; set; }
        public string? ListUserIdUnLike { get; set; }

        public CommentModelClient CastToClient()
        {
            CommentModelClient client = new CommentModelClient();

            client.Comment = Comment;
            client.Like = NoLike;
            client.UnLike = UnLike;
            client.Created= Created; 
            client.ListUserIdLike = ListUserIdLike;
            client.ListUserIdUnLike = ListUserIdUnLike;

            return client;
        }

    }

    public class CommentModelClient
    {
        public string? Id { get; set; }
        public string? FileId { get; set; }
        public string? UserId { get; set; }
        public string? Comment { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public DateTimeOffset Created { get; set; }

        public string? ListUserIdLike { get; set; }
        public string? ListUserIdUnLike { get; set; }

        public CommentModelServer CastToServer()
        {
            CommentModelServer server = new();

            server.Comment = Comment;
            server.NoLike = Like;
            server.UnLike = UnLike;
            server.Created = Created;
            server.ListUserIdLike = ListUserIdLike;
            server.ListUserIdUnLike= ListUserIdUnLike;

            return server;
        }
    }


}

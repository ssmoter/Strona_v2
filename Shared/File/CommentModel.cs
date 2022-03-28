namespace Strona_v2.Shared.File
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string? Comment { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public DateTimeOffset Created { get; set; }

        public string? ListUserIdLike { get; set; }
        public string? ListUserIdUnLike { get; set; }

    }
}

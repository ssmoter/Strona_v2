using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.File
{
    public class FileModelClient
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public string? UserId { get; set; }
        public int NoLike { get; set; }
        public int Spam { get; set; }
        public DateTimeOffset? Created { get; set; }
        public bool Main { get; set; }
        public string? Path { get; set; }
        public string? ListUserIdLike { get; set; }
        public string? ListUserIdSpam { get; set; }
        public string? StoredFileName { get; set; }
        public string? Type { get; set; }
        public List<FileSingleModel>? Files { get; set; }

        public FileModelServer CastToServer(FileModelClient client)
        {
            FileModelServer server = new();

            server.Title = client.Title;
            server.Description = client.Description;
            server.Tag = client.Tag;
            server.NoLike = client.NoLike;
            server.Spam = client.Spam;
            server.Created = client.Created;
            server.Main = client.Main;
            server.Path = client.Path;
            server.ListUserIdLike = client.ListUserIdLike;
            server.ListUserIdSpam = client.ListUserIdSpam;
            server.StoredFileName = client.StoredFileName;
            server.Type = client.Type;

            return server;
        }
    }

    public class FileModelServer
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public int UserId { get; set; }
        public int NoLike { get; set; }
        public int Spam { get; set; }
        public DateTimeOffset? Created { get; set; }
        public bool Main { get; set; }
        public string? Path { get; set; }
        public string? ListUserIdLike { get; set; }
        public string? ListUserIdSpam { get; set; }
        public string? StoredFileName { get; set; }
        public string? Type { get; set; }
        public List<FileSingleModel>? Files { get; set; }

        public FileModelClient CastToClient(FileModelServer server)
        {
            FileModelClient client = new();

            client.Title = server.Title;
            client.Description = server.Description;
            client.Tag = server.Tag;
            client.NoLike = server.NoLike;
            client.Spam = server.Spam;
            client.Created = server.Created;
            client.Main = server.Main;
            client.Path = server.Path;
            client.ListUserIdLike = server.ListUserIdLike;
            client.ListUserIdSpam = server.ListUserIdSpam;
            client.StoredFileName = server.StoredFileName;
            client.Type = server.Type;

            return client;
        }
    }



    public class FileSingleModel
    {
        public string? StoredFileName { get; set; }
        public string? Type { get; set; }

        public bool Uploaded { get; set; }
        public int ErrorCode { get; set; }
    }

    public class FileModelPublic
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public string? UserId { get; set; }
        public int NoLike { get; set; }
        public int Spam { get; set; }
        public DateTimeOffset? Created { get; set; }
        public bool Main { get; set; }
        public string? Path { get; set; }
        public string? ListUserIdLike { get; set; }
        public string? ListUserIdSpam { get; set; }
        public string? StoredFileName { get; set; }
        public string? Type { get; set; }
        public List<FileSingleModel>? Files { get; set; }

        public void TrimNameTyp()
        {
            var Name = StoredFileName.Split('/');
            var type = Type.Split('/');
            for (int i = 0; i < Name.Count(); i++)
            {
                Files.Add(new FileSingleModel() { StoredFileName = Name[i], Type = type[i] });
            }

        }

    }



}

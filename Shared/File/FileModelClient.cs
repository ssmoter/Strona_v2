using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.File
{
    public class FileModelC
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? Created { get; set; }
        public bool Main { get; set; }
        public string? StoredFileName { get; set; }
        public string? Type { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public List<FileSingleModel>? Files { get; set; }
        public FileModelC()
        {
            Files = new List<FileSingleModel>();
        }


    }

    public class FileModelClient : FileModelC
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? UserId { get; set; }

        public FileModelServer CastToServer(FileModelClient client)
        {
            FileModelServer server = new();

            server.Title = client.Title;
            server.Description = client.Description;
            server.Created = client.Created;
            server.Main = client.Main;
            server.StoredFileName = client.StoredFileName;
            server.Type = client.Type;

            return server;
        }
    }

    public class FileModelServer : FileModelC
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public FileModelClient CastToClient(FileModelServer server)
        {
            FileModelClient client = new();

            client.Title = server.Title;
            client.Description = server.Description;
            client.Created = server.Created;
            client.Main = server.Main;
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

    public class FileModelPublic : FileModelC
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }

        public void TrimNameTyp()
        {
            if (StoredFileName != null && Type != null)
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



}

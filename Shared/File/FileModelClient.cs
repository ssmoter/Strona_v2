using HashidsNet;
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
        public int NoLike { get; set; } = 0;
        public int UnLike { get; set; } = 0;
        public List<FileSingleModel>? Files { get; set; }
        public List<TagModelClient>? tagModels { get; set; }
        public ReactionModelClient? reactionModel { get; set; }
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

        public FileModelClient()
        { }
        public FileModelClient(FileModelServer server, IHashids hashids)
        {
            Title = server.Title;
            Description = server.Description;
            Created = server.Created;
            Main = server.Main;
            StoredFileName = server.StoredFileName;
            Type = server.Type;
            NoLike = server.NoLike;
            UnLike = server.UnLike;
            Files = server.Files;
            tagModels = server.tagModels;

            if (server.Id >= 0)
            {
                Id = hashids.Encode(server.Id);
            }
            if (server.UserId >= 0)
            {
                UserId = hashids.Encode(server.UserId);
            }
        }

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


        public FileModelServer()
        { }
        public FileModelServer(FileModelClient client, IHashids hashids)
        {
            Title = client.Title;
            Description = client.Description;
            Created = client.Created;
            Main = client.Main;
            StoredFileName = client.StoredFileName;
            Type = client.Type;
            NoLike = client.NoLike;
            UnLike = client.UnLike;
            Files = client.Files;
            tagModels = client.tagModels;

            if (!string.IsNullOrEmpty(client.Id))
            {
                Id = hashids.Decode(client.Id)[0];
            }
            if (!string.IsNullOrEmpty(client.UserId))
            {
                UserId = hashids.Decode(client.UserId)[0];
            }
        }

        public FileModelClient CastToClient(FileModelServer server)
        {
            FileModelClient client = new();
            client.UnLike = server.UnLike;
            client.NoLike = server.NoLike;
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

        public FileModelPublic()
        { }

        public FileModelPublic(FileModelServer server, IHashids hashids)
        {
            Title = server.Title;
            Description = server.Description;
            Created = server.Created;
            Main = server.Main;
            StoredFileName = server.StoredFileName;
            Type = server.Type;
            NoLike = server.NoLike;
            UnLike = server.UnLike;
            Files = server.Files;
            tagModels = server.tagModels;
            if (server.Id >= 0)
            {
                Id = hashids.Encode(server.Id);
            }
            if (server.UserId >= 0)
            {
                UserId = hashids.Encode(server.UserId);
            }

        }

        public static FileModelPublic SimpeCast(FileModelServer server, IHashids hashids)
        {
            if (server == null)
            {
                return null;
            }
            FileModelPublic modelPublic = new();
            if (server.Id >= 0)
            {
                modelPublic.Id = hashids.Encode(server.Id);
            }
            if (server.UserId >= 0)
            {
                modelPublic.UserId = hashids.Encode(server.UserId);
            }
            modelPublic.NoLike = server.NoLike;
            modelPublic.UnLike = server.UnLike;
            modelPublic.Created = server.Created;
            return modelPublic;
        }

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

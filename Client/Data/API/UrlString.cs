namespace Strona_v2.Client.Data.API
{
    public class UrlString
    {
        private string Main = "https://localhost:7249/api";

        public string User { get;  } = "/User/";
        public string File { get;  } = "/File/";
        public string Comment { get; } = "/Comment/";
        public string Tag { get; } = "/TagModel/";
        public UrlString()
        {
            User = Main + User;
            File =Main + File;
            Comment = Main + Comment;
            Tag = Main + Tag;
        }

    }
}

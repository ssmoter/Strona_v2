namespace Strona_v2.Client.Data.API
{
    public class UrlString
    {
        private string Main = "https://localhost:7249/api";

        public string User { get; set; } = "/User/";
        public string File { get; set; } = "/File/";

        public UrlString()
        {
            User = Main + User;
            File =Main + File;
        }

    }
}

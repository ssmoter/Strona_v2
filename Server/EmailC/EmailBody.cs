namespace Strona_v2.Server.EmailC
{
    public static class EmailBody
    {
        public static string RegisterTemplate(string email, string token)
        {
            string url = "https://localhost:7249/ConfirmEmail/";

            string template = "<p>Kod uwierzytelniający dla " + email + "</p>" +
                                "<p href= " + url + token + ">" + url + token + "</p>" +
                                "<p>Ważność tokenu 15min<p><p>Wiadomość wygenerowana automaczycznie.</p>";
            return template;
        }

    }
}

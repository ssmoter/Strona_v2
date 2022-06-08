using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.User
{
    public class UserRegisterClient
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "Przedział dlugości hasła to {2} do {1}", MinimumLength = 10)]
        [DataType(DataType.Password)]
        //Display(Name = "Password")]
        public string? Password { get; set; }

    }
    public class UserRegisterServer
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTimeOffset DataCreat { get; set; }
        public DateTimeOffset LastOnline { get; set; }
        public UserRegisterServer CastFromClient(UserRegisterClient client)
        {
            UserRegisterServer server = new();
            server.Name = client.Name;
            server.Email = client.Email;
            server.Password = client.Password;
            return server;
        }
    }
}

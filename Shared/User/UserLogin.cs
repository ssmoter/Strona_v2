using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.User
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string? SecondId { get; set; }

        public string? Name { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public string Token { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }


        public UserLogin CastFromUserRegisterServer(UserRegisterServer server)
        {
            UserLogin login = new();

            login.Email = server.Email;
            login.Password = server.Password;
            return login;
        }
    }

}

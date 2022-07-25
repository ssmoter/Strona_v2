using System.ComponentModel.DataAnnotations;

namespace Strona_v2.Shared.User
{
    public class UserPublic
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public int Like { get; set; }
        public int UnLike { get; set; }

        public DateTimeOffset LastOnline { get; set; }


        public DateTimeOffset DataCreat { get; set; }

        public string? AvatarNameString { get; set; }


    }
    public class UserLocalStorage
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Token { get; set; }
    }

}

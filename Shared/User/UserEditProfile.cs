using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Strona_v2.Shared.User
{
    public class UserEditProfile
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? AvatarNameString { get; set; }

    }
    public class UserEditProfileViewModel
    {
        public string? Name { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }

        public string? AvatarNameString { get; set; }
    }

}

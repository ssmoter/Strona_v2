using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strona_v2.Shared.User
{
    public class cToken
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }

        public cToken(string value, DateTimeOffset expiryDate)
        {
            Token = value;
            ExpiryDate = expiryDate;
        }

        public cToken()
        { }
    }
}

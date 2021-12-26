using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strona_v2.Shared.User
{
    public class Token
    {
        public string Value { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }

        public Token(string value, DateTimeOffset expiryDate)
        {
            Value = value;
            ExpiryDate = expiryDate;
        }

        public Token()
        { }
    }
}

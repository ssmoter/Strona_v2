using Strona_v2.Shared.User;

namespace Strona_v2.Server.EmailC
{
    public interface IEmailVerification
    {
        bool ConfirmToken(string token);
        string NewToken();
    }

    public class EmailVerification : IEmailVerification
    {
        private List<cToken> _emailsList;

        public EmailVerification()
        {
            _emailsList = new();
        }

        public string NewToken()
        {
            Random rnd = new();
            int max = 99_999999;
            int min = 10_000000;
            var token = new cToken
            {                
                Token = rnd.Next(min,max).ToString(),//8 cyfr
                ExpiryDate = DateTimeOffset.Now.AddMinutes(15)
            };
            _emailsList.Add(token);
            return token.Token;
        }

        public bool ConfirmToken(string token)
        {
            if (token == null)
            {
                return false;
            }
            if (_emailsList.Any(x => x.Token == token
            && x.ExpiryDate >= DateTimeOffset.Now))
            {
                return true;
            }
            return false;
        }



    }
}

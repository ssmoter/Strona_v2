using Strona_v2.Server.Data.SqlData;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.TokenAuthentication
{
    public class TokenManager : ITokenManager
    {
        private readonly ILoginUser _LogInUser;

        public TokenManager(ILoginUser logInUser)
        {
            _LogInUser = logInUser;
        }

        //w sumie to nie wiem po co mi to ale sprawdzenie czy dane istnieje
        public bool Authenticate(UserLogin user)
        {
            if (user != null)
            {
                return true;
            }
            return false;
        }
        //jezeli user się zalogował na nowo tworzony jest nowy token 
        //wraz z datą do kiedy będzie ważny 
        //data ustawiona na 10 dni 
        public Token NewToken(UserLogin user)
        {
            var token = new Token
            {
                Value = Guid.NewGuid().ToString(),
                ExpiryDate = DateTimeOffset.Now.AddDays(10),//ustawienie czasu istnienia tokenu
            };
            user.Token = token.Value;
            user.ExpiryDate = token.ExpiryDate;
            _LogInUser.SaveNewToken(user);

            return token;
        }

        //sprawdzenie czy token jest w bazie danych i czy data się zgadza
        public async Task<bool> VerifyToken(string token)
        {
            Token testToken = await _LogInUser.CheckTokenAndTime(token);

            if (testToken.Value == token
                && testToken.ExpiryDate > DateTimeOffset.Now)
            {
                return true;
            }
            return false;
        }
    }
}

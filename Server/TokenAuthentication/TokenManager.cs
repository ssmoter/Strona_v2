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
        public async Task<cToken> NewToken(UserLogin user)
        {
            var token = await LoadToken(user);

            if (token == null)
            {
                token = new cToken
                {
                    Token = Guid.NewGuid().ToString(),
                    ExpiryDate = DateTimeOffset.Now.AddDays(10),//ustawienie czasu istnienia tokenu
                };
            }
            user.Token = token.Token;
            user.ExpiryDate = token.ExpiryDate;
            _LogInUser.SaveNewToken(user);

            return token;
        }
        private async Task<cToken> LoadToken(UserLogin user)
        {
            cToken token = await _LogInUser.CheckTokenAndTime(user.Email, user.Password);
            if (token != null && token.Token != "")
            {
                return token;
            }
            return null;
        }



        //sprawdzenie czy token jest w bazie danych i czy data się zgadza
        public async Task<bool> VerifyToken(string token)
        {
            //ktos nie podał tokena
            if (token == null)
            {
                return false;
            }
            cToken testToken = await _LogInUser.CheckTokenAndTime(token);
            if (testToken == null && testToken.Token != "")
            {
                return false;
            }

            if (testToken.Token == token
                && testToken.ExpiryDate > DateTimeOffset.Now)
            {
                return true;
            }
            return false;
        }
    }
}

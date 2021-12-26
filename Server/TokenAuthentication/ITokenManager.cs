using Strona_v2.Shared.User;

namespace Strona_v2.Server.TokenAuthentication
{
    public interface ITokenManager
    {
        bool Authenticate(UserLogin user);
        Token NewToken(UserLogin user);
        Task<bool> VerifyToken(string token);
    }
}
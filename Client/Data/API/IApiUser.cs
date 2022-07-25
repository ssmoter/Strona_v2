using Strona_v2.Shared.User;

namespace Strona_v2.Client.Data.API
{
    public interface IApiUser
    {
        Task<int> IndividualEmail(ILogger logger, string Email);
        Task<UserLogin?> LogIn(string email, string password);
        Task<UserPublic?> ProfileUserPublicID(string Id, ILogger logger);
        Task<UserPublic> ProfileUserPublicName(string UserName, ILogger logger);
        Task<UserLogin?> Register(ILogger logger, UserRegisterClient client);
    }
}
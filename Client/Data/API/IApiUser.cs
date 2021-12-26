using Strona_v2.Shared.User;

namespace Strona_v2.Client.Data.API
{
    public interface IApiUser
    {
        string ApiStringName { get; set; }

        Task<UserLogin?> LogIn(string email, string password);
        Task<UserPublic> ProfileUserPublic(int Id);
        Task<UserPublic> ProfileUserPublic(string UserName);
    }
}
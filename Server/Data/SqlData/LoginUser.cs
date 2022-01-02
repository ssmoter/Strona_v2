using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Data.SqlData
{
    public interface ILoginUser
    {
        Task<cToken> CheckTokenAndTime(string token);
        Task<cToken> CheckTokenAndTime(string Email, string Password);
        void SaveNewToken(UserLogin user);
        Task<UserLogin> UserLoginAsync(string Email, string Password);
    }

    public class LoginUser : ILoginUser
    {
        private readonly ISqlDataAccess _dataAccess;
        public LoginUser(ISqlDataAccess sqlDataAccess)
        {
            _dataAccess = sqlDataAccess;
        }

        //pobieranie danych do logowania
        public async Task<UserLogin> UserLoginAsync(string Email, string Password)
        {
            string sql = "SELECT Id,Email,Name " +
                " FROM dbo.UserData " +
                " WHERE Email = N'" + Email + "' AND Password = N'" + Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";
            return await _dataAccess.LoadData<UserLogin>(sql);
        }

        //zapisanie tokena 
        public async void SaveNewToken(UserLogin user)
        {
            string sql = "UPDATE dbo.UserData " +
                " SET Token = @Token , ExpiryDate = @ExpiryDate " +
                "WHERE Email= N'" + user.Email +
                "'AND Id = " + user.Id;

            await _dataAccess.SaveData(sql, user);
        }

        //sprawdzienie czy token istnieje i jego czas jest poprawny
        public async Task<cToken> CheckTokenAndTime(string token)
        {
            string sql = "SELECT Token,ExpiryDate " +
                " FROM dbo.UserData " +
                " WHERE Token = N'" + token + "'";

            return await _dataAccess.LoadData<cToken>(sql);
        }
        public async Task<cToken> CheckTokenAndTime(string Email, string Password)
        {
            string sql = "SELECT Token,ExpiryDate " +
                " FROM dbo.UserData " +
                " WHERE Email = N'" + Email + "' AND Password = N'" + Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            return await _dataAccess.LoadData<cToken>(sql);
        }
    }
}

using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Data.SqlData
{
    public interface ILoginUser
    {
        Task<cToken> CheckTokenAndTime(string Token);
        Task<cToken> CheckTokenAndTime(string Email, string Password);
        Task<IList<string>> IndividualEmail();
        Task Register(UserRegisterServer server);
        void SaveNewToken(UserLogin user);
        Task<UserLogin> UserLoginAsync(string Email, string Password);
    }

    public class LoginUser : ILoginUser
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly string TableName = "UserData";

        public LoginUser(ISqlDataAccess sqlDataAccess)
        {
            _dataAccess = sqlDataAccess;
        }

        //pobieranie danych do logowania
        public async Task<UserLogin> UserLoginAsync(string Email, string Password)
        {
            string sql = "SELECT Id,Email,Name " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(Email) + " = N'" + Email + "' AND " + nameof(Password) + " = N'" + Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";
            return await _dataAccess.LoadData<UserLogin>(sql);
        }

        //zapisanie tokena 
        public async void SaveNewToken(UserLogin user)
        {
            string sql = "UPDATE dbo." + TableName +
                " SET " + nameof(user.Token) + " = @" + nameof(user.Token) + " , " + nameof(user.ExpiryDate) + " = @" + nameof(user.ExpiryDate) + " " +
                " WHERE Email= N'" + user.Email +
                " 'AND Id = " + user.Id;

            await _dataAccess.SaveData(sql, user);
        }

        //sprawdzienie czy token istnieje i jego czas jest poprawny
        public async Task<cToken> CheckTokenAndTime(string Token)
        {
            string sql = "SELECT Token,ExpiryDate " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(Token) + " = N'" + Token + "'";

            return await _dataAccess.LoadData<cToken>(sql);
        }
        public async Task<cToken> CheckTokenAndTime(string Email, string Password)
        {
            string sql = "SELECT Token,ExpiryDate " +
                " FROM dbo." + TableName +
                " WHERE " + nameof(Email) + " = N'" + Email + "' AND " + nameof(Password) + " = N'" + Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            return await _dataAccess.LoadData<cToken>(sql);
        }

        //pobieranie Emaili do sprawdzenia czy już taki istnieje
        public async Task<IList<string>> IndividualEmail()
        {
            string sql = "SELECT  " + nameof(UserEditProfile.Email) +
                " FROM dbo." + TableName;

            return await _dataAccess.LoadDataList<string>(sql);
        }

        //Rejestracja
        public async Task Register(UserRegisterServer server)
        {
            string sql = "INSERT INTO dbo." + TableName +
                "\n (" + nameof(UserRegisterServer.Name) + ", " + nameof(UserRegisterServer.Email) + ", " + nameof(UserRegisterServer.Password) + "," + nameof(UserRegisterServer.DataCreat) + "," + nameof(UserRegisterServer.LastOnline) +
                ")\n VALUES\n (@" +
                nameof(UserRegisterServer.Name) + ", @" + nameof(UserRegisterServer.Email) + ", @" + nameof(UserRegisterServer.Password) + ", @" + nameof(UserRegisterServer.DataCreat) + ", @" + nameof(UserRegisterServer.LastOnline) + ")";

            await _dataAccess.SaveData(sql, server);
        }

    }
}

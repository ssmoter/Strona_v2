using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Data.SqlData
{
    public interface IEditProfileUser
    {
        Task EditAvatarNameString(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditEmail(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditName(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditPassword(UserLogin loginUser, UserEditProfile userEditProfile);
    }

    public class EditProfileUser : IEditProfileUser
    {
        private readonly ISqlDataAccess _SqlDataAccess;

        public EditProfileUser(ISqlDataAccess sqlDataAccess)
        {
            _SqlDataAccess = sqlDataAccess;
        }
        //zmiana Email
        public async Task EditEmail(UserLogin loginUser, UserEditProfile userEditProfile)
        {
            string sql = "UPDATE dbo.UserData " +
                " SET Email = @Email , EmailConfirm = 0" +
                " WHERE Email = N'" + loginUser.Email + "' AND Password = N'" + loginUser.Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            await _SqlDataAccess.SaveData(sql, userEditProfile);
        }
        //zmiana nick
        public async Task EditName(UserLogin loginUser, UserEditProfile userEditProfile)
        {
            string sql = "UPDATE dbo.UserData " +
                " SET Name = @Name " +
                " WHERE Email = N'" + loginUser.Email + "' AND Password = N'" + loginUser.Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            await _SqlDataAccess.SaveData(sql, userEditProfile);
        }
        //zmiana avataru
        public async Task EditAvatarNameString(UserLogin loginUser, UserEditProfile userEditProfile)
        {
            string sql = "UPDATE dbo.UserData " +
                " SET AvatarNameString = @AvatarNameString " +
                " WHERE Email = N'" + loginUser.Email + "' AND Password = N'" + loginUser.Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            await _SqlDataAccess.SaveData(sql, userEditProfile);
        }
        //zmiania hasła
        public async Task EditPassword(UserLogin loginUser, UserEditProfile userEditProfile)
        {
            string sql = "UPDATE dbo.UserData " +
                " SET Password = @Password " +  //zmienić na Hash jak będzie działacć
                " WHERE Email = N'" + loginUser.Email + "' AND Password = N'" + loginUser.Password + "'";
            //"HASHBYTES('SHA2_512',CONVERT(NVARCHAR(32),N'" + Password + "'))";

            await _SqlDataAccess.SaveData(sql, userEditProfile);
        }


    }
}

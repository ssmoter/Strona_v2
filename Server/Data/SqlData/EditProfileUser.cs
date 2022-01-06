﻿using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Data.SqlData
{
    public interface IEditProfileUser
    {
        Task EditAvatarNameString(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditEmail(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditName(UserLogin loginUser, UserEditProfile userEditProfile);
        Task EditPassword(UserLogin loginUser, UserEditProfile userEditProfile);
        Task<bool> UpdatelastOnline(string token, DateTimeOffset LastOnline);
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
        //aktualizowanie kiedy ostatnio był dostępny user
        public async Task<bool> UpdatelastOnline(string token, DateTimeOffset LastOnline)
        {
            if (!(await CheckToken(token)))
            {
                return false;
            }
            UserPublic user = new();
            user.LastOnline = LastOnline;
            string sql = "UPDATE dbo.UserData " +
                " SET LastOnline = @LastOnline" +
                " WHERE Token = N'" + token + "'";

            await _SqlDataAccess.SaveData(sql, user);
            return true;
        }
        private async Task<bool> CheckToken(string token)
        {
            string sql = "SELECT Id,Email,Name " +
                    " FROM dbo.UserData " +
                      " WHERE Token = N'" + token + "'";
            UserLogin login = new();
            login = await _SqlDataAccess.LoadData<UserLogin>(sql);
            if (login != null)
            {
                return true;
            }
            return false;
        }
    }
}

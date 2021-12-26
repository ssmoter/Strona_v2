using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;

namespace Strona_v2.Server.Data.SqlData
{
    public interface IProfileUser
    {
        Task<UserPublic> UserFullDataIntId(int Id);
        Task<UserPublic> UserFullDataStringName(string Name);
    }

    public class ProfileUser : IProfileUser
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ProfileUser(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        //Pobieranie nie prywatnych danych po nr konta
        public async Task<UserPublic> UserFullDataIntId(int Id)
        {
            string sql = "SELECT * " +
                " FROM dbo.UserData " +
                " WHERE Id = " + Id;

            return await _sqlDataAccess.LoadData<UserPublic>(sql);
        }

        //Pobieranie nie prywatnych danych po nazwie konta
        public async Task<UserPublic> UserFullDataStringName(string Name)
        {
            string sql = "SELECT * " +
                " FROM dbo.UserData " +
                " WHERE Name = N'" + Name + "'";

            return await _sqlDataAccess.LoadData<UserPublic>(sql);
        }
    }
}

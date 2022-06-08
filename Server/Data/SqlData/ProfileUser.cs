using Strona_v2.Shared.SqlDataAccess;
using Strona_v2.Shared.User;
using System.Xml.Linq;

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
        private readonly string Tablename = "UserData";
        public ProfileUser(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        //Pobieranie nie prywatnych danych po nr konta
        public async Task<UserPublic> UserFullDataIntId(int Id)
        {
            string sql = "SELECT * " +
                " FROM dbo." + Tablename +
                " WHERE " + nameof(Id) + " = " + Id;

            return await _sqlDataAccess.LoadData<UserPublic>(sql);
        }

        //Pobieranie nie prywatnych danych po nazwie konta
        public async Task<UserPublic> UserFullDataStringName(string Name)
        {
            string sql = "SELECT * " +
                " FROM dbo." + Tablename +
                " WHERE " + nameof(Name) + " = N'" + Name + "'";

            return await _sqlDataAccess.LoadData<UserPublic>(sql);
        }
    }
}

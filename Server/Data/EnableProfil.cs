namespace Strona_v2.Server.Data
{
    public enum EnableProfilEnum
    {
        Success,
        Error,
        Name=211,
        Email,
    }

    public class EnableProfil
    {
        public List<string> ListName { get; set; }
        public List<string> ListEmail { get; set; }
        public EnableProfil()
        {        }
        public EnableProfil(List<string> listEmail, List<string> listName)
        {
            ListEmail = listEmail;
            ListName = listName;
        }

        public bool EnableName(string Parametr)
        {
            for (int i = 0; i < ListName.Count; i++)
            {
                if (ListName[i]==Parametr)
                {
                    return true;
                }
            }
            return false;
        }
        public bool EnableEmail(string Parametr)
        {
            for (int i = 0; i < ListEmail.Count; i++)
            {
                if (ListEmail[i] == Parametr)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

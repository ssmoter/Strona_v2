namespace Strona_v2.Server.Data.SqlCreatTable
{
    public class ExampleTable
    {
        public string? TableName { get; set; }
        public string[]? ColumnName { get; set; }
        public string[]? ColumnParametr { get; set; }

        public ExampleTable(int ColumnNameSize)
        {
            ColumnName = new string[ColumnNameSize];
            ColumnParametr = new string[ColumnNameSize];
        }

        //szkielet zapytania jak wygląda tabela
        public string CreatSqlTable(string? TableType, string? TableName)
        {
            string start = string.Format("USE Database_strona_v2 " +
                        " CREATE TABLE [dbo].[{0}_{1}]\n(\n", TableType, TableName);
            string mid = "	[Id] INT NOT NULL PRIMARY KEY IDENTITY, \n";
            for (int i = 0; i < ColumnName.Count(); i++)
            {
                mid += string.Format("[{0}] {1} null,\n", ColumnName[i], ColumnParametr[i]);
            }
            string end = start + mid + ")";

            return end;
        }

        //usunięcie tabeli
        public string DeleteSqlTable(string? TableName, string? TableType)
        {
            string sql = string.Format("DROP TABLE dbo.{0}_{1}", TableType, TableName);
            return sql;
        }

    }
}

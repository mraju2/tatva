namespace database_analysis.Models
{
    public class DatabaseConnectionDetails
    {
        public string ProviderName { get; set; } // e.g., System.Data.SqlClient, Npgsql, Oracle.ManagedDataAccess.Client, MongoDB.Driver, Microsoft.Data.Sqlite
        public string Server { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            return ProviderName switch
            {
                "System.Data.SqlClient" => $"Server={Server},{Port};Database={DatabaseName};User Id={Username};Password={Password};",
                "Npgsql" => $"Host={Server};Port={Port};Database={DatabaseName};Username={Username};Password={Password};",
                "Oracle.ManagedDataAccess.Client" => $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Server})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={DatabaseName})));User Id={Username};Password={Password};",
                "Microsoft.Data.Sqlite" => $"Data Source={DatabaseName};",
                "MongoDB.Driver" => $"mongodb://{Username}:{Password}@{Server}:{Port}/{DatabaseName}",
                "MySql.Data.MySqlClient" => $"Server={Server};Port={Port};Database={DatabaseName};User={Username};Password={Password};",
                _ => throw new NotSupportedException("The specified provider is not supported.")
            };
        }
    }
}


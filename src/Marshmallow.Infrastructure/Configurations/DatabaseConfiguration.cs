namespace Marshmallow.Infrastructure.Configurations;

public class DatabaseConfiguration
{
    public const string SectionName = "Database";

    public string? User { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public string? Port { get; set; }
    public string? Database { get; set; }
    public string? DataSource { get; set; }
    public DatabaseProvider Provider { get; set; }
}

public enum DatabaseProvider
{ 
    Mysql,
    Sqlite,
    Postgres,
}

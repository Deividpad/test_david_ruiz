using api_rest.Enum;

namespace api_rest.Db;

public class ConnectionStringProvider
{
    private readonly string? _connectionString;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(AppSettings.CONNECTION_DB_APP_SETTINGS);
    }

    public string GetConnectionString()
    {
        return _connectionString ?? throw new InvalidOperationException("Connection string not found.");
    }
}
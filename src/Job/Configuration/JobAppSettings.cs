using Shared.Models;

namespace Job.Configuration;

public record JobAppSettings : AppSettings
{
    public PostgresSettings PostgresSettings { get; init; }
        = new("", "", "", "", 0);
}

public record PostgresSettings(
    string Host,
    string Username,
    string Password,
    string Database,
    int Port);
using Microsoft.Data.SqlClient;

namespace VRT.MssqlTools.Decrypt.Cli.Models;

public sealed record class AppParameters : ICommandParameterSet
{
    /// <summary>
    /// Sql server name. Name format: [server]\[instance]
    /// </summary>
    [Option('s', Description = @"Sql server name. Name format: [server]\[instance]")]
    required public string SqlServer { get; init; }

    /// <summary>
    /// Sql server database name
    /// </summary>
    [Option('d', Description = "Sql server database name")]
    required public string SqlDatabase { get; init; }

    /// <summary>
    /// Sql server user name. If this value is empty, windows authentication will be used.
    /// </summary>
    [Option('u', Description = "Sql server user name. If this value is empty, windows authentication will be used.")]
    [HasDefaultValue]
    public string? SqlUserName { get; init; } = "";

    /// <summary>
    /// Sql server user password. Ignored if SqlUserName is empty.
    /// </summary>
    [Option('p', Description = "Sql server user password. Ignored if SqlUserName is empty.")]
    [HasDefaultValue]
    public string? SqlPassword { get; init; } = "";

    /// <summary>
    /// Sql server user password. Ignored if SqlUserName is empty.
    /// </summary>
    [Option('n', Description = "Object name filter pattern. No case sensitive.")]
    [HasDefaultValue]
    public string? ObjectNameFilter { get; init; } = "";

    public string ToConnectionString()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = $"admin:{SqlServer}",
            InitialCatalog = SqlDatabase,
            IntegratedSecurity = string.IsNullOrEmpty(SqlUserName),
            UserID = SqlUserName,
            Password = SqlPassword,
            TrustServerCertificate = true,
            MultipleActiveResultSets = true
        };
        var result = connectionStringBuilder.ToString();
        return result;
    }

}

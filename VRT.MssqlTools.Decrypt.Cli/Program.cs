using Dapper;
using Microsoft.Data.SqlClient;
using VRT.MssqlTools.Decrypt.Cli.Extensions;

CoconaLiteApp.Run(Execute);

void Execute(AppParameters option)
{
    var connectionString = option.ToConnectionString();
    using SqlConnection connection = new(connectionString);
    connection.Open();

    var hasNameFilter = option.ObjectNameFilter.IsEmpty().Not();
    var dbInfo = connection.QueryFirst<SqlDbInfo>(SqlDbInfo.SqlQuery);

    connection
        .Query<SqlSysObject>(SqlSysObject.SqlQuery)
        .Where(o => o.ImageVal is not null && o.ImageVal.Length > 0)
        .WhereIf(hasNameFilter, o => o.Name.IsEmptyOrMatch(option.ObjectNameFilter!))
        .Select(o => o.Decrypt(dbInfo))
        .NonEmpty()
        .ToList()
        .ForEach(Console.WriteLine);
}

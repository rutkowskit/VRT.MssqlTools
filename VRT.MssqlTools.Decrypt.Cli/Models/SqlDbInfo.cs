namespace VRT.MssqlTools.Decrypt.Cli.Models;

public sealed class SqlDbInfo
{
    required public int DbId { get; init; }
    required public byte[] FamilyGuid { get; init; }

    public static readonly string SqlQuery = """
    SELECT 
        DB_ID() as DbId,
        CONVERT(binary(16), DRS.family_guid) as FamilyGuid
    FROM sys.database_recovery_status AS DRS
    WHERE DRS.database_id = DB_ID();
    """;
}

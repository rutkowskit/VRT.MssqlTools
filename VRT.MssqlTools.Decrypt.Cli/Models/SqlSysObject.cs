using System.Security.Cryptography;
using VRT.MssqlTools.Decrypt.Cli.Extensions;

namespace VRT.MssqlTools.Decrypt.Cli.Models;

public sealed class SqlSysObject
{
    required public string Name { get; init; }
    required public byte[] ImageVal { get; init; }
    required public byte[] ObjectId { get; init; }
    required public byte[] SubObjectId { get; init; }

    public static readonly string SqlQuery = """
    SELECT 
        o.[Name] as Name,        
        s.imageval as ImageVal,    
        CONVERT(binary(4), REVERSE(CONVERT(binary(4), s.objid))) as ObjectId,
        CONVERT(binary(2), REVERSE(CONVERT(binary(2), s.subobjid))) as SubObjectId        
    FROM sys.sysobjvalues AS s    
    INNER JOIN sys.objects as o on s.[objId] = o.[object_id]
    LEFT JOIN sys.sql_modules sm ON o.[object_id] = sm.[object_id]
    WHERE 1=1
    AND s.valclass in (1, 5)    
    AND o.type IN ('P', 'V', 'FN', 'TF', 'IF')
    ORDER by 1;
    """;

    public string? Decrypt(SqlDbInfo dbInfo)
    {
        var imageValText = System.Text.Encoding.UTF8.GetString(ImageVal);
        if (imageValText.HasCreationSyntax())
        {
            return imageValText;
        }
        var key = ToRC4Key(dbInfo);
        var decrypted = ImageVal.EncryptOrDecryptRC4(key); //OK
        imageValText = System.Text.Encoding.Unicode.GetString(decrypted);

        return imageValText.HasCreationSyntax()
            ? imageValText
            : null;
    }
    private byte[] ToRC4Key(SqlDbInfo sqlDbInfo)
    {
        var toHash = sqlDbInfo.FamilyGuid.Concat(ObjectId).Concat(SubObjectId).ToArray();
        return SHA1.HashData(toHash);
    }
}

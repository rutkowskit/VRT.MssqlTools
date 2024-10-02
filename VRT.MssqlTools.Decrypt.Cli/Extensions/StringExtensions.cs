namespace VRT.MssqlTools.Decrypt.Cli.Extensions;

internal static class StringExtensions
{
    public static bool HasCreationSyntax(this string? text)
        => text?.Contains("create", StringComparison.InvariantCultureIgnoreCase) ?? false;

    public static bool IsEmpty(this string? text) => string.IsNullOrWhiteSpace(text);

    public static bool IsEmptyOrMatch(this string? text, string textPart) => text.IsEmpty() ||
        text!.Contains(textPart, StringComparison.InvariantCultureIgnoreCase);

    public static bool MatchIf(this string? text, bool shouldMatch, string textPart) => text switch
    {
        _ when shouldMatch.Not() => true,
        null when textPart.IsEmpty() => true,
        null => false,
        var t => t.Contains(textPart, StringComparison.InvariantCultureIgnoreCase)
    };
}

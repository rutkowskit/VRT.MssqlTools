namespace VRT.MssqlTools.Decrypt.Cli.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> items, bool shouldFilter, Func<T, bool> predicate)
        => shouldFilter ? items.Where(predicate) : items;

    public static IEnumerable<T> NonEmpty<T>(this IEnumerable<T?> items)
        => items.Where(i => i != null).Select(i => i!);
}

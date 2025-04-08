namespace Configuration.Misc;

internal static class ResultExtensions
{
    public static Result<List<TValue>, TError> Collect<TValue, TError>(this IEnumerable<Result<TValue, TError>> results) where TError : notnull where TValue : notnull
    {
        return results.TakeUntil(result => result.IsErr).ToList().Combine();
    }

    private static Result<List<TValue>, TError> Combine<TValue, TError>(this List<Result<TValue, TError>> results) where TError : notnull where TValue : notnull
    {
        var last = results.Last();

        return last.IsErr
            ? Result<List<TValue>, TError>.Err(last.ExpectError())
            : Result<List<TValue>, TError>.Ok(results.Select(result => result.Unwrap()).ToList());
    }

    private static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        using var enumerator = source.GetEnumerator();
        var ok = true;

        while (ok && enumerator.MoveNext())
        {
            var current = enumerator.Current;
            yield return current;
            ok = !predicate(current);
        }
    }
}
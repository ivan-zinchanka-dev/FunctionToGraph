using System.Diagnostics.Contracts;

namespace Domain.Extensions;

public static class StringExtensions
{
    [Pure]
    public static string WithFramingQuotes(this string source)
    {
        return $"\"{source}\"";
    }
    
    [Pure]
    public static string WithoutFramingQuotes(this string source)
    {
        return source.Trim('"');
    }
}
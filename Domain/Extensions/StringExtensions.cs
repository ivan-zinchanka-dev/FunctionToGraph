using System.Diagnostics.Contracts;

namespace Domain.Extensions;

// TODO quotes and double quotes cases to Csv reader/writer
// TODO Set normal culture for doubles
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
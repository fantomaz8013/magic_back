using System.Text.RegularExpressions;

namespace Magic.DAL.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerCaseWithUnderscore(this string value)
        {
            return Regex
                .Replace(value, "(?<=[^_])[A-Z]", result => '_' + result.ToString().ToLower())
                .ToLower();
        }
    }
}

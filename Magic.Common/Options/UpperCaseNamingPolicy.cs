using System.Text.Json;

namespace Magic.Common.Options
{
    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public static UpperCaseNamingPolicy Instance { get; } = new UpperCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            return ToSnakeCase(name);
        }

        public static string ToSnakeCase(string str)
        {
            return str.ToUpper();
        }
    }
}

using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text.RegularExpressions;

namespace Codesanook.Examples.DotNet.Orm.EF6Examples
{
    public class SnakecaseModelBasedConvention : Convention
    {
        public SnakecaseModelBasedConvention()
        {
            // https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
            Properties().Configure(x => x.HasColumnName(""));
        }
    }

    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}

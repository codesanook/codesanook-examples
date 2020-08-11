using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text.RegularExpressions;

namespace Codesanook.Examples.DotNetCore.Orm.EFExamples
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }

    public class ModelBasedConvention : Convention
    {
        public ModelBasedConvention()
        {
            // https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
            Properties().Configure(x => x.HasColumnName(""));
        }
    }
}

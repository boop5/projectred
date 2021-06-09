using System.Text.RegularExpressions;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Utilities
{
    internal sealed class EshopSlugBuilder : SlugBuilder, IEshopSlugBuilder
    {
        public override string Build(string input)
        {
            var output = input;

            // remove "for nintendo switch"
            output = Regex.Replace(output, @"for[\s]+nintendo[\s]+switch", string.Empty, RegexOptions.IgnoreCase);
            output = base.Build(output);

            return output;
        }
    }
}
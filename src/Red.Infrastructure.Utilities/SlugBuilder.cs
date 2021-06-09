using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Utilities
{
    internal class SlugBuilder : ISlugBuilder
    {
        public virtual string Build(string input)
        {
            // todo: check performance, probably very inefficient method
            var output = input;

            // remove multiple space
            output = Regex.Replace(output, "[ ]{2,}", " ");

            // convert roman to arabic numerals
            output = RomanToArabic(output);

            // remove diacritics (áèîô)
            output = RemoveDiacritics(output);

            // remove colon
            output = Regex.Replace(output, @"([^\s])(:+)([^\s])", "$1-$3");
            output = Regex.Replace(output, ":+", string.Empty);

            // remove non alphanumeric characters
            output = Regex.Replace(output, @"[^a-zA-Z0-9 -]+", string.Empty);

            // trim space
            output = output.Trim();

            // replace space characters with dash (-)
            output = Regex.Replace(output, @"\s+", "-");

            // to lowercase
            output = output.ToLowerInvariant();

            // remove multiple dashes
            output = Regex.Replace(output, "[-]{2,}", "-");

            // remove non alphanumeric characters at start and end
            output = Regex.Replace(output, "^([^a-z0-9]+)([a-z0-9 -]+?)([^a-z0-9]+)$", "$2");

            return output;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private string RomanToArabic(string input)
        {
            int? Roman2Arabic(string s)
            {
                return s switch
                {
                    "II" => 2,
                    "III" => 3,
                    "IV" => 4,
                    "V" => 5,
                    "VI" => 6,
                    "VII" => 7,
                    "VIII" => 8,
                    "IX" => 9,
                    "X" => 10,
                    "XI" => 11,
                    "XII" => 12,
                    "XIII" => 13,
                    "XIV" => 14,
                    "XV" => 15,
                    "XVI" => 16,
                    "XVII" => 17,
                    "XVIII" => 18,
                    "XIX" => 19,
                    _ => null
                };
            }

            var output = input;

            // todo: this is a quick&dirty solution and kinda needs a refactoring
            var parts = Regex.Split(output, @"\b");
            var result = new List<string>(parts.Length);
            foreach (var part in parts)
            {
                var n = Roman2Arabic(part);
                result.Add(n != null ? n.ToString() : part);
            }

            output = string.Join("", result);

            return output;
        }
    }
}
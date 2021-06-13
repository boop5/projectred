using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{HexCode,nq}")]
    public sealed class HexColor
    {
        private static readonly Regex Regex = new("^[#]{0,1}[a-fA-F0-9]{6}$", RegexOptions.Compiled);

        public string HexCode { get; private init; }

        public HexColor(string hexCode)
        {
            if (!IsValidCode(hexCode))
            {
                throw new ArgumentException($"invalid hex code {hexCode}");
            }

            HexCode = Normalize(hexCode);
        }

        public static bool IsValidCode(string hexCode)
        {
            return Regex.IsMatch(hexCode);
        }

        private static string Normalize(string hexCode)
        {
            var normalized = hexCode;

            if (!normalized.StartsWith("#"))
            {
                normalized = $"#{normalized}";
            }

            normalized = normalized.ToLowerInvariant().Trim();

            return normalized;
        }

        #region Equality

        private bool Equals(HexColor other)
        {
            return string.Equals(HexCode, other.HexCode, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is HexColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HexCode.GetHashCode();
        }

        #endregion
    }
}
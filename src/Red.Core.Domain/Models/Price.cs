using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Amount:F} {Currency}")]
    public sealed record Price
    {
        public float Amount { get; init; }
        public string Currency { get; init; } = "";

        public override string ToString()
        {
            return $"{Amount:F} {Currency}";
        }
    }
}
using System;

namespace Red.Core.Domain.Models
{
    public sealed class PriceRecord
    {
        public DateTime Date { get; init; }
        public decimal Price { get; init; }
    }
}
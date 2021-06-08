namespace Red.Core.Domain.Models
{
    public sealed class EshopPriceQuery
    {
        public string Nsuid { get; }

        public EshopPriceQuery(string nsuid)
        {
            Nsuid = nsuid;
        }
    }
}
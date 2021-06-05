using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EzNintendo.Data.Converter
{
    internal class NsuidToLongConverter : ValueConverter<NsuId, long>
    {
        public NsuidToLongConverter()
            : base(x => x.Id, x => new NsuId(x)) { }
    }
}
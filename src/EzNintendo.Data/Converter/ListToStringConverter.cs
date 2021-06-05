using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EzNintendo.Data.Converter {
    internal sealed class ListToStringConverter : ValueConverter<List<string>, string>
    {
        public ListToStringConverter(string separator)
            : base(v => string.Join(separator, v),
                   v => v.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList()) { }
    }
}
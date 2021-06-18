using System.Collections.Generic;
using System.Globalization;
using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Queries
{
    public sealed class GetGamesFromEshopQuery : IQuery<IReadOnlyList<SwitchGame>>
    {
        public CultureInfo Culture { get; }
        public int QuerySize { get; }

        public GetGamesFromEshopQuery(CultureInfo culture, int querySize = 200)
        {
            Culture = culture;
            QuerySize = querySize;
        }
    }
}
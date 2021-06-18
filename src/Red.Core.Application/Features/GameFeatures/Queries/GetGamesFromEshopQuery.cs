using System.Collections.Generic;
using System.Globalization;
using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Queries
{
    public sealed class GetGamesFromEshopQuery : IQuery<IReadOnlyList<SwitchGame>>
    {
        public CultureInfo Culture { get; }

        public GetGamesFromEshopQuery(CultureInfo culture)
        {
            Culture = culture;
        }
    }
}

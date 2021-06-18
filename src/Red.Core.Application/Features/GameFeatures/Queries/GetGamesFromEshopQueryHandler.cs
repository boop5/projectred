using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Queries
{
    internal sealed class GetGamesFromEshopQueryHandler : IRequestHandler<GetGamesFromEshopQuery, IReadOnlyList<SwitchGame>>
    {
        private readonly IEshop _eshop;

        public GetGamesFromEshopQueryHandler(IEshop eshop)
        {
            _eshop = eshop;
        }

        public async Task<IReadOnlyList<SwitchGame>> Handle(GetGamesFromEshopQuery request, CancellationToken cancellationToken)
        {
            var result = new List<SwitchGame>();
            var queries = await BuildQueries(request.Culture, 200);

            foreach (var query in queries)
            {
                var games = await _eshop.SearchGames(query);
                result.AddRange(games);
            }

            return result;
        }

        private async Task<IReadOnlyCollection<EshopGameQuery>> BuildQueries(CultureInfo culture, int querySize)
        {
            var end = await _eshop.GetTotalGames(culture);
            var queries = new List<EshopGameQuery>();

            for (var start = 0; start < end; start += querySize)
            {
                queries.Add(new EshopGameQuery(culture) {Index = start, Offset = querySize});
            }

            return queries;
        }

        /*private async Task ProcessQuery(EshopGameQuery query)
        {
            try
            {
                var repo = _repoFactory.Create();
                var games = await _eshop.SearchGames(query);
                Log.LogDebug("Process {count} games", games.Count);

                foreach (var game in games)
                {
                    await UpdateGame(query.Culture, repo, game);
                }
            }
            catch (Exception e)
            {
                // Log.LogWarning(e, "Failed to process query {query}", query);
            }
        }*/

    }
}

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Queries
{
    internal sealed class GetGameQueryHandler : IRequestHandler<GetGameQuery, SwitchGame?>
    {
        private readonly ISwitchGameRepositoryFactory _repositoryFactory;

        public GetGameQueryHandler(ISwitchGameRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<SwitchGame?> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            var repo = _repositoryFactory.Create();
            var game = await repo.GetByFsId(request.FsId);

            return game;
        }
    }
}
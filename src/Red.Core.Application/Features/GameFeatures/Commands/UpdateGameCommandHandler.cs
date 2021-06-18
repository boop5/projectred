using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Features.GameFeatures.Events;
using Red.Core.Application.Interfaces;

namespace Red.Core.Application.Features.GameFeatures.Commands
{
    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand>
    {
        private readonly ISwitchGameMerger _gameMerger;
        private readonly ISwitchGameRepository _repo;
        private IAppLogger<UpdateGameCommandHandler> Log { get; }

        public UpdateGameCommandHandler(IAppLogger<UpdateGameCommandHandler> log, ISwitchGameRepository repo, ISwitchGameMerger gameMerger)
        {
            Log = log;
            _repo = repo;
            _gameMerger = gameMerger;
        }

        public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbEntity = await _repo.GetByFsId(request.Game.FsId);

                if (dbEntity == null)
                {
                    // todo: handle slug issue (minefield ...)
                    await _repo.AddAsync(request.Game);
                }
                else
                {
                    var updatedEntity = _gameMerger.MergeLibrary(dbEntity!, request.Game);

                    if (!Equals(dbEntity, updatedEntity))
                    {
                        await _repo.UpdateAsync(updatedEntity);
                    }
                }
            }
            catch (Exception e)
            {
                var title = GameAddedEventHandler.GetTitle(request.Game);
                Log.LogWarning(e, "Failed to update game '{game}' ({fsId})", title, request.Game.FsId);
            }

            return Unit.Value;
        }
    }
}
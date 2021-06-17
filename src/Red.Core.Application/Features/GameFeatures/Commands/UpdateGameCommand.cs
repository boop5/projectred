using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Commands
{
    public class UpdateGameCommand : ICommand
    {
        public SwitchGame Game { get; }

        public UpdateGameCommand(SwitchGame game)
        {
            Game = game;
        }
    }
}

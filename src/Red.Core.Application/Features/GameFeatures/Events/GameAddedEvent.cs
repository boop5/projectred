using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Events
{
    public sealed class GameAddedEvent : IEvent
    {
        public SwitchGame Game { get; }

        public GameAddedEvent(SwitchGame game)
        {
            Game = game;
        }
    }
}
using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Events
{
    public sealed class GameUpdatedEvent : IEvent
    {
        public SwitchGame Game { get; }

        public GameUpdatedEvent(SwitchGame game)
        {
            Game = game;
        }
    }
}
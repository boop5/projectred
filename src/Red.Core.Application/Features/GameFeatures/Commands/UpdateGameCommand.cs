using System.Globalization;
using Red.Core.Application.Common;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Commands
{
    public class UpdateGameCommand : ICommand
    {
        public CultureInfo Culture { get; }
        public SwitchGame Game { get; }

        public UpdateGameCommand(CultureInfo culture, SwitchGame game)
        {
            Culture = culture;
            Game = game;
        }
    }
}

using FluentValidation;

namespace Red.Core.Application.Features.GameFeatures.Commands
{
    public sealed class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
    {
        public UpdateGameCommandValidator()
        {
            RuleFor(x => x.Game.FsId)
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage("FsId can't be null or empty");

            RuleFor(x => x.Game.Title[x.Culture.TwoLetterISOLanguageName])
                .NotEmpty()
                .WithSeverity(Severity.Error)
                .WithMessage("Title can't be null or empty");

            RuleForEach(x => x.Game.Colors)
                .NotEmpty()
                .WithSeverity(Severity.Warning)
                .WithMessage("A color can't be null or empty");
        }
    }
}
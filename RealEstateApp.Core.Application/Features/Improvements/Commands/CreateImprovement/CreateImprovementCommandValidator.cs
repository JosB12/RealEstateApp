
using FluentValidation;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    public class CreateImprovementCommandValidator : AbstractValidator<CreateImprovementCommand>
    {
        public CreateImprovementCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The improvement name is required");

            RuleFor(p => p.Description)
              .NotEmpty().WithMessage("The description is required");

        }
    }
}

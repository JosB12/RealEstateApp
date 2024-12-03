
using FluentValidation;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    public class UpdateImprovementCommandValidator : AbstractValidator<UpdateImprovementCommand>
    {
        public UpdateImprovementCommandValidator()
        {
            RuleFor(p => p.Id)
                 .NotEmpty().WithMessage("The improvement id is required")
                 .GreaterThan(0).WithMessage("The improvement id must be greater of 0");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The improvement name is required");

            RuleFor(p => p.Description)
                 .NotEmpty().WithMessage("The description id is required");
        }
    }
}

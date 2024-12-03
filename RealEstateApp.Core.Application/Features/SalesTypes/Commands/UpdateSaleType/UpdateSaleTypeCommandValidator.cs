
using FluentValidation;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Commands.UpdateSaleType
{
    public class UpdateSaleTypeCommandValidator : AbstractValidator<UpdateSaleTypeCommand>
    {
        public UpdateSaleTypeCommandValidator()
        {
            RuleFor(p => p.Id)
                 .NotEmpty().WithMessage("The sale type id is required")
                 .GreaterThan(0).WithMessage("The sale type id must be greater of 0");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The sale type name is required");

            RuleFor(p => p.Description)
                 .NotEmpty().WithMessage("The description id is required");
        }
    }
}

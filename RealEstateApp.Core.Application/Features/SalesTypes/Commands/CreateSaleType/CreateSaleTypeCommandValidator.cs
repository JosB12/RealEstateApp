
using FluentValidation;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Commands.CreateSaleType
{
   
    public class CreateSaleTypeCommandValidator : AbstractValidator<CreateSaleTypeCommand>
    {
        public CreateSaleTypeCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The Sale Type name is required");

            RuleFor(p => p.Description)
              .NotEmpty().WithMessage("The description is required");

        }
    }
}


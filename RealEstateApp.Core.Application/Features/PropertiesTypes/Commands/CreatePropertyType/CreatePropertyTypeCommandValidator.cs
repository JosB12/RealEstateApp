using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType
{
    public class CreatePropertyTypeCommandValidator : AbstractValidator<CreatePropertyTypeCommand>
    {
        public CreatePropertyTypeCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The Property Type name is required");

            RuleFor(p => p.Description)
              .NotEmpty().WithMessage("The description is required");

        }
    }
}

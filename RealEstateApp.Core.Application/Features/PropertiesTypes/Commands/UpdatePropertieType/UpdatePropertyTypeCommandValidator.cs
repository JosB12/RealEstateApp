using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType
{
    public class UpdatePropertyTypeCommandValidator : AbstractValidator<UpdatePropertyTypeCommand>
    {
        public UpdatePropertyTypeCommandValidator()
        {
            RuleFor(p => p.Id)
                 .NotEmpty().WithMessage("The property type id is required")
                 .GreaterThan(0).WithMessage("The property type id must be greater of 0");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The property type name is required");

            RuleFor(p => p.Description)
                 .NotEmpty().WithMessage("The description id is required");
        }
    }
}

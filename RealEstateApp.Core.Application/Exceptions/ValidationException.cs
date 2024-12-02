using FluentValidation.Results;

namespace RealEstateApp.Core.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Se ha producido uno o más fallos de validación.")
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; }
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }

    }
}

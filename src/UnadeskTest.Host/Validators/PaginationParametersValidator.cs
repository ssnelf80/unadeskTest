using FluentValidation;
using UnadeskTest.Host.Models;

namespace UnadeskTest.Host.Validators;

public sealed class PaginationParametersValidator : AbstractValidator<PaginationParameters>
{
    public PaginationParametersValidator()
    {
        RuleFor(p => p.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset must be greater or equal 0");
        
        RuleFor(p => p.Limit)
            .InclusiveBetween(0, 100)
            .WithMessage("Limit must be between 0 and 100");
    }
}
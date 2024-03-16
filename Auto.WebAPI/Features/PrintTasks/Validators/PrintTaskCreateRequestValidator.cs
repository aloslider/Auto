using Auto.WebAPI.Features.PrintTasks.Requests;
using FluentValidation;

namespace Auto.WebAPI.Features.PrintTasks.Validators;

class PrintTaskCreateRequestValidator : AbstractValidator<PrintTaskCreateRequest>
{
    public PrintTaskCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Task name must be non empty");

        RuleFor(x => x.PageCount)
            .GreaterThan(0).WithMessage("Page count must be positive");
    }
}

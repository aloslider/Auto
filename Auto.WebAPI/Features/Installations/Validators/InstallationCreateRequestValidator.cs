using Auto.WebAPI.Features.Installations.Requests;
using FluentValidation;

namespace Auto.WebAPI.Features.Installations.Validators;

class InstallationCreateRequestValidator : AbstractValidator<InstallationCreateRequest>
{
    public InstallationCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Max length exceeded");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("Branch name is required");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required");

        RuleFor(x => x.OrderNumber)
            .InclusiveBetween(1, 255).WithMessage("Order number should be in range of [1, 255]");
    }
}

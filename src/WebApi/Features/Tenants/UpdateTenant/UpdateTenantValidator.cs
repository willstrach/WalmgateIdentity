using FluentValidation;

namespace WalmgateIdentity.WebApi.Features.Tenants.UpdateTenant;

public class UpdateTenantValidator : AbstractValidator<UpdateTenantRequest>
{
    public UpdateTenantValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .When(request => request.Name is not null);
    }
}

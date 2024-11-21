namespace WalmgateIdentity.WebApi.Features.Tenants.CreateTenant;

using FluentValidation;

public class CreateTenantValidator : AbstractValidator<CreateTenantRequest>
{
    public CreateTenantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 50);
    }
}

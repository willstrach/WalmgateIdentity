using FluentValidation;

namespace WalmgateIdentity.WebApi.Features.Tenants.ListTenants;

public class ListTenantsValidator : AbstractValidator<ListTenantsRequest>
{
    public ListTenantsValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0);

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(request => request.Search)
            .MaximumLength(50);
    }
}

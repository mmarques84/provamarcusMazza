using FluentValidation;

namespace provamarcusMazza.Application.Customers.Queries.GetCustomers;

public sealed class GetCustomersValidator : AbstractValidator<GetCustomersQuery>
{
    public GetCustomersValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

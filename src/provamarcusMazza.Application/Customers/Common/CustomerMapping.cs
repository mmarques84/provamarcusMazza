using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Customers.Common;

internal static class CustomerMapping
{
    public static CustomerResponse ToResponse(this Customer customer)
        => new(customer.Id, customer.Name, customer.Email, customer.CreatedAt);
}

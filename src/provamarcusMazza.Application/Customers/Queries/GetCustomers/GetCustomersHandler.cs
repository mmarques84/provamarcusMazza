using MediatR;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Customers.Common;

namespace provamarcusMazza.Application.Customers.Queries.GetCustomers;

public sealed class GetCustomersHandler(ICustomerRepository customerRepository)
    : IRequestHandler<GetCustomersQuery, PagedResult<CustomerResponse>>
{
    public async Task<PagedResult<CustomerResponse>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var (customers, totalCount) = await customerRepository.GetPagedAsync(
            request.Page, request.PageSize, cancellationToken);

        return new PagedResult<CustomerResponse>(
            customers.Select(c => c.ToResponse()).ToList(),
            request.Page,
            request.PageSize,
            totalCount);
    }
}

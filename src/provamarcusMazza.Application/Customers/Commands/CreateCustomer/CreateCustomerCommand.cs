using MediatR;
using provamarcusMazza.Application.Customers.Common;

namespace provamarcusMazza.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string Email)
    : IRequest<CustomerResponse>;

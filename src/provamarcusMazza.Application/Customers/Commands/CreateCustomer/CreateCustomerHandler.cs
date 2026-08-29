using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Customers.Common;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (await customerRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException("A customer with this email is already registered.");

        var customer = new Customer(Guid.NewGuid(), request.Name, request.Email);

        await customerRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToResponse();
    }
}

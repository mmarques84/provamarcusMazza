using FluentValidation;

namespace provamarcusMazza.Application.Orders.Commands.CancelOrder;

public sealed class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

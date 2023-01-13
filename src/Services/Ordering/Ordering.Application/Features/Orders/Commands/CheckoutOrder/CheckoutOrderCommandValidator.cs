using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(p => p.EmailAddress)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.TotalPrice)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
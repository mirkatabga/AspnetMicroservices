using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();

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
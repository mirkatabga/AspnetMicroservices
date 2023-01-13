using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        private readonly IOrderRepository _repo;

        public DeleteOrderCommandHandler(
            ILogger<DeleteOrderCommandHandler> logger,
            IOrderRepository repo)
        {
            _logger = logger; 
            _repo = repo;   
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);

            if (order is null)
            {
                _logger.LogError("Order not found");
            }

            await _repo.DeleteAsync(order!);
            _logger.LogInformation($"Order {order!.Id} deleted.");

            return Unit.Value;
        }
    }
}
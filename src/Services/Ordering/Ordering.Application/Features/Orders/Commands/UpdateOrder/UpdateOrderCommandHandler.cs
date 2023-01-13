using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repo;

        public UpdateOrderCommandHandler(
            ILogger<UpdateOrderCommandHandler> logger,
            IMapper mapper,
            IOrderRepository repo)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;    
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);

            if (order is null)
            {
                _logger.LogError("Order not found.");
            }

            _mapper.Map(request, order, typeof(UpdateOrderCommand), typeof(Order));

            await _repo.UpdateAsync(order!);

            _logger.LogInformation($"Order {order!.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
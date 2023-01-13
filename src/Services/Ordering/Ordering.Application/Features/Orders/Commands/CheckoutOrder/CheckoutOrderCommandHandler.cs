using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repo;
        private readonly IEmailService _emailService;

        public CheckoutOrderCommandHandler(
            ILogger<CheckoutOrderCommandHandler> logger,
            IMapper mapper,
            IOrderRepository repo,
            IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;
            _emailService = emailService;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            var persistedOrder = await _repo.AddAsync(order);

            _logger.LogInformation($"Order {persistedOrder.Id} was persisted successfully");

            await SendMailAsync(persistedOrder);

            return persistedOrder.Id;
        }

        private async Task SendMailAsync(Order order)
        {
            var email = new Email
            {
                To = "mirkatabga@gmail.com",
                Body = $"Order with id: {order.Id} was created.",
                Subject = $"New Order #{order.Id}"
            };

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Email with order {order.Id} couldn't be sent.");
            }
        }
    }
}
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderVm>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repo;

        public GetOrdersListQueryHandler(
            IMapper mapper,
            IOrderRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<List<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetAsync(
                order => order.UserName == request.Username);

            return _mapper.Map<List<OrderVm>>(orders);
        }
    }
}
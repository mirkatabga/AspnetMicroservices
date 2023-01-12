using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderVm>>
    {
        public GetOrdersListQuery(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }

        public string Username { get; set; }
    }
}
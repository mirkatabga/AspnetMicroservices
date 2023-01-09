using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _repo;

        public DiscountService(
            ILogger<DiscountService> logger,
            IMapper mapper,
            IDiscountRepository repo)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repo.GetDiscountAsync(request.ProductName);

            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for the product: {request.ProductName}"));
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }
    }
}
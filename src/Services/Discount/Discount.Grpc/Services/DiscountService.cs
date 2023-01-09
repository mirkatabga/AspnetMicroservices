using AutoMapper;
using Discount.Grpc.Entities;
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
            _logger.LogTrace($"{nameof(GetDiscount)} invoked");

            var coupon = await _repo.GetDiscountAsync(request.ProductName);

            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for the product: {request.ProductName}"));
            }

            _logger.LogTrace($"Discount Coupon retrieved. {nameof(coupon.ProductName)}: {coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"{nameof(CreateDiscount)} invoked");

            var coupon = _mapper.Map<Coupon>(request.Coupon);
            bool success = await _repo.CreateDiscountAsync(coupon);

            if(success)
            {
                _logger.LogInformation($"Discount Coupon created. {nameof(coupon.ProductName)}: {coupon.ProductName}");
            }

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"{nameof(UpdateDiscount)} invoked");

            var coupon = _mapper.Map<Coupon>(request.Coupon);
            var success = await _repo.UpdateDiscountAsync(coupon);

            if(success)
            {
                _logger.LogInformation($"Discount Coupon updated. {nameof(coupon.ProductName)}: {coupon.ProductName}");
            }

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            _logger.LogTrace($"{nameof(DeleteDiscount)} invoked");

            var success = await _repo.DeleteDiscountAsync(request.ProductName);

            if(success)
            {
                _logger.LogInformation($"Discount Coupon deleted. {nameof(request.ProductName)}: {request.ProductName}");
            }

            return new DeleteDiscountResponse
            {
                Success = success
            };
        }
    }
}
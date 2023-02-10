using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Basket.API.GrpcServices;
using EventBus.Messages.Events;
using AutoMapper;
using MassTransit;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repo;
        private readonly DiscountGrpcService _discountService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(
            IBasketRepository repo,
            DiscountGrpcService discountService,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repo = repo;
            _discountService = discountService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string username)
        {
            var basket = await _repo.GetBasketAsync(username);

            if (basket is null)
            {
                return Ok(new ShoppingCart(username));
            }

            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountService.GetDiscountAsync(item.ProductName!);

                item.Price -= coupon.Amount;
            }

            var updatedBasket = await _repo.UpdateBasketAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete("{username}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _repo.DeleteBasketAsync(username);

            return Ok();
        }

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repo.GetBasketAsync(basketCheckout.UserName!);

            if (basket is null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);
            await _repo.DeleteBasketAsync(basket.Username!);

            return Accepted();
        }
    }
}
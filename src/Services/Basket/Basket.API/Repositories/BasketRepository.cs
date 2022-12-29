using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;
        
        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ShoppingCart?> GetBasketAsync(string username)
        {
            var basket = await _cache.GetStringAsync(username);

            if (string.IsNullOrWhiteSpace(basket))
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            if(string.IsNullOrWhiteSpace(basket.Username))
            {
                throw new ArgumentException(
                    $"Null or empty property: {nameof(basket)}.{nameof(ShoppingCart.Username)}");
            }

            await _cache.SetStringAsync(
                basket.Username, 
                JsonSerializer.Serialize(basket));

            return basket;
        }

        public async Task DeleteBasketAsync(string username)
        {
            await _cache.RemoveAsync(username);
        }
    }
}
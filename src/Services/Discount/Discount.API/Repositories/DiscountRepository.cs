using System.Data;
using Dapper;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDbConnection _connection;

        public DiscountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", 
                new { ProductName = productName });

            if (coupon is null)
            {
                coupon = new Coupon 
                { 
                    ProductName = "No Discount",
                    Amount = 0, 
                    Description = "No Discount Desc" 
                };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            var affected = await _connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            return affected > 0;
        }
    }
}
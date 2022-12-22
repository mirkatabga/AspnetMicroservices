using System.Net;
using Microsoft.AspNetCore.Mvc;
using Services.Catalog.Catalog.API.Entities;
using Services.Catalog.Catalog.API.Repositories;

namespace Services.Catalog.Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;

        public CatalogController(
            ILogger<CatalogController> logger,
            IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetProductsAsync();

            return Ok(products);
        }
    }
}
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Catalog.API.Entities;
using Catalog.API.Repositories;

namespace Catalog.API.Controllers
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

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _repository.GetProductAsync(id);

            if(product is null)
            {
                _logger.LogInformation($"Product with id: {id}, not found.");
                return NotFound(id);
            }

            return Ok(product);
        }

        [HttpGet("[action]/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductByCategoryAsync(category);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProductAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            bool isModified = await _repository.UpdateProductAsync(product);

            return Ok(isModified);
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            bool isDeleted = await _repository.DeleteProductAsync(id);
            return Ok(isDeleted);
        }
    }
}
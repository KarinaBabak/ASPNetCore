using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET api/v1/product
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetAll();
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        // GET api/2/product
        [HttpGet, MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<IActionResult> GetTwoTopProducts()
        {
            var products = await _productService.GetAll();
            
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products.Take(2));
        }

        // GET api/v1/product
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST api/v1/product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductDto product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var createdProductId = await _productService.Add(product);
            product.Id = createdProductId;

            return Created(Request.Path, product);
        }

        // PUT: api/product/id
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BadRequestResult), 404)]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        public async Task<IActionResult> Update(int id, ProductDto product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.Update(product);
            return NoContent();
        }

        // Delete api/v1/product/id
        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        public async Task<ActionResult<ProductDto>> Delete(int productId)
        {
            var foundProduct = await _productService.GetById(productId);

            if (foundProduct == null)
            {
                return NotFound();
            }

            await _productService.Delete(foundProduct);

            return NoContent();
        }

    }
}
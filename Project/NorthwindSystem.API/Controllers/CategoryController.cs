using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET api/1/category
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var categories = await _categoryService.GetAll();
            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet]
        [Route("image/{categoryId}")]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(IEnumerable<byte>), 200)]
        public async Task<ActionResult> GetImageById(int categoryId)
        {
            var image = await _categoryService.GetImage(categoryId);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpPost]
        [Route("image/{categoryId}")]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(typeof(OkResult), 200)]
        public async Task<ActionResult> UpdateImageById(int categoryId, [FromBody] byte[] imageBytes)
        {
            if (imageBytes == null)
            {
                return BadRequest();
            }

            var category = await _categoryService.GetById(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateImage(categoryId, imageBytes);

            return Ok();
        }
    }
}
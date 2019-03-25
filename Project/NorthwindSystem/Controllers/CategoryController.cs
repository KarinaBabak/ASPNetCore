using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindSystem.BLL.Interface;

namespace NorthwindSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private const string _imageContentType = "image/bmp";

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAll();

            return View(categories);
        }

        public async Task<IActionResult> GetImage(int categoryId)
        {
            var image = await _categoryService.GetImage(categoryId);

            if (image == null)
            {
                return NotFound();
            }
            var stream = new MemoryStream(image);
            return File(stream, _imageContentType);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int categoryId)
        {
            ViewBag.Method = "Update Picture";
            var category = await _categoryService.GetById(categoryId);
            return View("UpdatePictureView", category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int categoryId, IFormFile picture)
        {
            if (picture != null)
            {
                using (var stream = new MemoryStream())
                {
                    await picture.CopyToAsync(stream);
                    await _categoryService.UpdateImage(categoryId, stream.ToArray());
                }
            }      
            
            return RedirectToAction("Index");
        }

    }
}
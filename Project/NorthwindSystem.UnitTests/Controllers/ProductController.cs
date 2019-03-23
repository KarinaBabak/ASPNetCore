using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Controllers;
using NorthwindSystem.Data.DTOModels;
using Xunit;

namespace NorthwindSystem.UnitTests.Controllers
{
    public class ProductControllerUnitTests
    {
        private readonly ProductController _controller;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ISupplierService> _supplierServiceMock;

        public ProductControllerUnitTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _supplierServiceMock = new Mock<ISupplierService>();
            _controller = new ProductController(_productServiceMock.Object, _categoryServiceMock.Object, _supplierServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedProducts());

            _categoryServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedCategories());

            _supplierServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedSuppliers());

            // Act
            var result = await _controller.Index();
            //ViewResult result = _controller.Index() as ViewResult;

            // Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //Assert.Equal("Hello world!", result?.ViewData["Message"]);
        }

        private IEnumerable<ProductDto> GetMockedProducts()
        {
            return new List<ProductDto>
            {
                new ProductDto
                {
                    Id = 1,
                    CategoryName = "CategoryName1",
                    UnitPrice = 7,
                },
                new ProductDto
                {
                    Id = 2,
                    CategoryName = "CategoryName1",
                    UnitPrice = 10,
                },
            };
        }

        private IEnumerable<CategoryDto> GetMockedCategories()
        {
            return new List<CategoryDto>
            {
                new CategoryDto
                {
                    Id = 1,
                    Name = "CategoryName1",
                    Description = "Description",
                },
                new CategoryDto
                {
                    Id = 2,
                    Name = "CategoryName2",
                    Description = "Description",
                },
            };
        }

        private IEnumerable<SupplierDto> GetMockedSuppliers()
        {
            return new List<SupplierDto>
            {
                new SupplierDto
                {
                    Id = 1,
                    Name = "Supplier Name1",
                    ContactName = "Test Name",
                },
                new SupplierDto
                {
                    Id = 2,
                    Name = "Supplier Name2",
                    ContactName = "Test Contact Name",
                },
            };
        }
    }
}

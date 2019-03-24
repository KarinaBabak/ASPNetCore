using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Controllers;
using NorthwindSystem.Data.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindSystem.UnitTests
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ISupplierService> _supplierServiceMock;

        public ProductControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _supplierServiceMock = new Mock<ISupplierService>();
            _controller = new ProductController(_productServiceMock.Object, _categoryServiceMock.Object, _supplierServiceMock.Object);
        }

        [Fact]
        public async Task Index_ProductServiceGetAllCalled()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedProducts());

            // Act
            var result = await _controller.Index();
            //ViewResult result = _controller.Index() as ViewResult;

            // Assert
            _productServiceMock.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Index_ResultNotNull()
        {
            // Arrange
            _productServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedProducts());

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.NotNull(result);
        }

        #region UpdateTests
        [Fact]
        public async Task Update_FetchAllData()
        {
            // Arrange
            int productId = 1;

            // Act
            var result = await _controller.Update(productId);

            // Assert
            _productServiceMock.Verify(service => service.GetById(productId), Times.Once);
            _categoryServiceMock.Verify(service => service.GetAll(), Times.Once);
            _supplierServiceMock.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Update_SaveUpdatedValidModel()
        {
            // Arrange
            var editedProduct = new ProductDto();

            // Act
            var result = await _controller.Update(editedProduct);

            // Assert
            _productServiceMock.Verify(x => x.Update(editedProduct), Times.Once);
        }

        [Fact]
        public async Task Update_RedirectToIndex_SaveUpdatedValidModel()
        {
            // Arrange
            var editedProduct = new ProductDto();

            // Act
            var result = await _controller.Update(editedProduct);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Update_NotSaveInValidModel()
        {
            // Arrange
            var editedProduct = new ProductDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Update(editedProduct);

            // Assert
            _productServiceMock.Verify(service => service.Update(editedProduct), Times.Never);
        }

        [Fact]
        public async Task Update_RedirectToUpdate_InValidModel()
        {
            // Arrange
            var editedProduct = new ProductDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Update(editedProduct);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Update", redirectToActionResult.ActionName);
        }
        #endregion

        #region AddTests
        [Fact]
        public async Task Add_FetchAllData()
        {
            // Act
            var result = await _controller.Add();

            // Assert
            _categoryServiceMock.Verify(service => service.GetAll(), Times.Once);
            _supplierServiceMock.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Add_SaveValidModel()
        {
            // Arrange
            var newProduct = new ProductDto();

            // Act
            var result = await _controller.Add(newProduct);

            // Assert
            _productServiceMock.Verify(x => x.Add(newProduct), Times.Once);
        }

        [Fact]
        public async Task Add_RedirectToIndex_SaveValidModel()
        {
            // Arrange
            var newProduct = new ProductDto();

            // Act
            var result = await _controller.Add(newProduct);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Add_NotSaveNotExistedModel()
        {
            // Act
            var result = await _controller.Add(null);

            // Assert
            _productServiceMock.Verify(service => service.Add(null), Times.Never);
        }

        [Fact]
        public async Task Add_RedirectToAdd_InValidModel()
        {
            // Arrange
            var editedProduct = new ProductDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Add(editedProduct);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Add", redirectToActionResult.ActionName);
        }
        #endregion

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

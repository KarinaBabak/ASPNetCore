using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Controllers;
using NorthwindSystem.Data.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindSystem.UnitTests
{
    public class CategoryControllerTest
    {
        private readonly CategoryController _controller;
        private readonly Mock<ICategoryService> _categoryServiceMock;

        public CategoryControllerTest()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task Index_GetAllCalled()
        {
            // Arrange
            _categoryServiceMock.Setup(service => service.GetAll())
                .ReturnsAsync(GetMockedCategories());

            // Act
            var result = await _controller.Index();

            // Assert
            _categoryServiceMock.Verify(service => service.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetImage_One_FetchImage()
        {
            // Arrange
            var categoryId = 1;
            
            // Act
            await _controller.GetImage(categoryId);

            // Assert
            _categoryServiceMock.Verify(x => x.GetImage(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetImage_ReturnImage()
        {
            // Arrange
            var categoryId = 1;
            _categoryServiceMock.Setup(x => x.GetImage(categoryId)).ReturnsAsync(new byte[] { });

            // Act
            var result = await _controller.GetImage(1);

            // Assert
            Assert.IsType<FileStreamResult>(result);
        }

        [Fact]
        public async Task GetImage_NotFoundResult()
        {
            // Arrange
            var categoryId = 1;
            _categoryServiceMock.Setup(x => x.GetImage(categoryId)).ReturnsAsync((byte[])null);

            // Act
            var result = await _controller.GetImage(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ExistingPicture_Save()
        {
            // Arrange
            var categoryId = 1;
            var bytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            // Act
            var result = await _controller.Update(categoryId, null);

            // Assert
            _categoryServiceMock.Verify(x => x.UpdateImage(categoryId, bytes), Times.Never);
        }

        [Fact]
        public async Task Update_NotExistingPicture_NotSave()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = await _controller.Update(categoryId, null);

            // Assert
            _categoryServiceMock.Verify(x => x.UpdateImage(categoryId, (byte[])null), Times.Never);
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
    }
}

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

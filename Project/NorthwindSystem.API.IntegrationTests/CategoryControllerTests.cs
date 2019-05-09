using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindSystem.Data.DTOModels;
using Xunit;
namespace NorthwindSystem.API.IntegrationTests
{
    public class CategoryControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;

        public CategoryControllerTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task GetAllCategoriesAsync_v1_should_return_200()
        {
            // Arrange
            var request = "/api/v1/Category";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetImageByIdAsync_v1_should_return_200()
        {
            // Arrange
            var request = "/api/v1/Category/image/";
            var getAllCategoriesRequest = "/api/v1/Category";

            var getAllCategoriesResponse = await Client.GetAsync(getAllCategoriesRequest);
            var value = await getAllCategoriesResponse.Content.ReadAsStringAsync();

            var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(value);
            if (categories != null)
            {
                request += categories.FirstOrDefault().Id;
            }

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetImageByIdAsync_v1_should_return_NotFound()
        {
            // Arrange
            var request = "/api/v1/Category/image/-100";

            // Act
            var response = await Client.GetAsync(request);
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task UpdateImageByIdAsync_v1_should_return_NotFound()
        {
            // Arrange
            var requestUrl = "/api/v1/Category/image/-10";
            byte[] updatedImage = new byte[2] { 0x20, 0x20 };
            var json = JsonConvert.SerializeObject(updatedImage);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync(requestUrl, payload);
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task UpdateImageByIdAsync_v1_should_return_BadRequest()
        {
            // Arrange
            var requestUrl = "/api/v1/Category/image/10";
            var json = JsonConvert.SerializeObject(null);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync(requestUrl, payload);
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task UpdateImageByIdAsync_v1_should_return_Successfully()
        {
            // Arrange
            var requestUrl = "/api/v1/Category/image/";
            var getAllCategoriesRequestUrl = "/api/v1/Category";
            byte[] updatedImage = new byte[2] { 0x20, 0x20 };

            var json = JsonConvert.SerializeObject(updatedImage);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            var getAllCategoriesResponse = await Client.GetAsync(getAllCategoriesRequestUrl);
            var value = await getAllCategoriesResponse.Content.ReadAsStringAsync();

            var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(value);
            if (categories != null)
            {
                requestUrl += categories.FirstOrDefault().Id;
            }

            // Act
            var response = await Client.PostAsync(requestUrl, payload);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}

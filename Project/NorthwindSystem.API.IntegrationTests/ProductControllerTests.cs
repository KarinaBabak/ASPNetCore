using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindSystem.Data.DTOModels;
using Xunit;


namespace NorthwindSystem.API.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;

        public ProductControllerTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task GetAllProductsAsync_v1_should_return_200()
        {
            // Arrange
            var request = "/api/v1/Product";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAllProductsAsync_v2_should_return_200()
        {
            // Arrange
            var request = "/api/v2/Product";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(2, response.RequestMessage.Version.Major);
        }

        [Fact]
        public async Task GetProductByIdAsync_should_return_200()
        {
            // Arrange
            string productTestName = "Test Product";
            var request = new
            {
                Url = "/api/v1/Product",
                Body = new ProductDto
                {
                    Name = productTestName,
                    UnitPrice = 5
                }
            };

            var createResponse = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await createResponse.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<ProductDto>(value);
            var requestUrl = $"/api/v1/Product/{singleResponse.Id}";

            // Act
            var response = await Client.GetAsync(requestUrl);

            // Assert
            var statusCode = response.StatusCode;
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetProductByIdAsync_should_return_product_by_id()
        {
            // Arrange
            string productTestName = "Test Product";
            var request = new
            {
                Url = "/api/v1/Product",
                Body = new ProductDto
                {
                    Name = productTestName,
                    UnitPrice = 5
                }
            };

            var createResponse = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var createdValue = await createResponse.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<ProductDto>(createdValue);
            var requestUrl = $"/api/v1/Product/{singleResponse.Id}";

            // Act
            var response = await Client.GetAsync(requestUrl);
            var value = await response.Content.ReadAsStringAsync();
            var foundProduct = JsonConvert.DeserializeObject<ProductDto>(value);

            // Assert
            Assert.Equal(productTestName, foundProduct.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_should_return_NotFound()
        {
            // Arrange
            var request = "/api/v1/Product/-500";

            // Act
            var response = await Client.GetAsync(request);
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        #region Create Product
        [Fact]
        public async Task CreateProductAsync_should_save_successfully()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/Product",
                Body = new ProductDto
                {
                    Name = "Test Product",
                    UnitPrice = 5
                }
            };

            // Act
            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var actualStatusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.Created, actualStatusCode);
        }

        [Fact]
        public async Task CreateProductAsync_null_product_should_return_Bad_Request()
        {
            // Arrange
            var requestUrl = "/api/v1/Product";

            // Act
            var response = await Client.PostAsync(requestUrl, ContentHelper.GetStringContent(null));
            var actualStatusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, actualStatusCode);
        }
        #endregion

        #region Update Product
        [Fact]
        public async Task UpdateProductAsync_incorrect_id_should_return_BadRequest()
        {
            // Arrange
            int id = -1;
            var request = new
            {
                Url = $"/api/v1/Product/{id}",
                Body = new ProductDto
                {
                    Id = id - 1,
                    Name = "Test Product",
                    UnitPrice = 5
                }
            };

            // Act
            var updateResponse = await Client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            var actualStatusCode = updateResponse.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, actualStatusCode);
        }

        [Fact]
        public async Task UpdateProductAsync_should_return_NoContentResult()
        {
            // Arrange
            var product = new ProductDto
            {
                Name = "Test Product",
                UnitPrice = 5
            };

            var request = new
            {
                Url = "/api/v1/Product",
                Body = product
            };

            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<ProductDto>(value);

            product.Name += "Updated";
            product.Id = singleResponse.Id;

            // Act
            var updateResponse = await Client.PutAsync(string.Format("/api/v1/Product/{0}", singleResponse.Id), ContentHelper.GetStringContent(product));

            // Assert
            updateResponse.EnsureSuccessStatusCode();
        }
        #endregion

        #region Delete Product
        [Fact]
        public async Task DeleteProductAsync_should_return_NoContentResult()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/Product",
                Body = new ProductDto
                {
                    Name = "Test Product",
                    UnitPrice = 5
                }
            };

            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<ProductDto>(value);

            // Act
            var deleteResponse = await Client.DeleteAsync(string.Format("/api/v1/Product/{0}", singleResponse.Id));
            var actualStatusCode = deleteResponse.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actualStatusCode);
        }

        [Fact]
        public async Task DeleteProductAsync_not_existed_product_should_return_Not_Found()
        {
            // Arrange
            var requestUrl = "/api/v1/Product/-1";

            // Act
            var response = await Client.DeleteAsync(requestUrl);
            var actualStatusCode = response.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, actualStatusCode);
        }
        #endregion
    }
}

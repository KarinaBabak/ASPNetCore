using NorthwindSystem.Data.DTOModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NorthwindSystem.WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string categoryUri = "/api/v1/category";
            const string productUriVersion1 = "/api/v1/product";
            const string productUriVersion2 = "/api/v2/product";
            const string baseUri = "http://localhost:44308";
            ProductDto defaultProduct = new ProductDto
            {
                CategoryId = 1,
                Name = "Console product",
                UnitPrice = 5
            };

            HttpClient httpClient = CreateHttpClient(baseUri);
            //var uriResult = CreateAsync(httpClient, productUriVersion1, defaultProduct).GetAwaiter().GetResult();

            //Console.WriteLine($"Result of creating product: {0}", uriResult);
            //Console.ReadLine();

            var productsV1 = GetListDataAsync<ProductDto>(httpClient, productUriVersion1).GetAwaiter().GetResult();
            DisplayProducts(productsV1);
            Console.ReadLine();

            Console.WriteLine("Api Version 2");
            var productsV2 = GetListDataAsync<ProductDto>(httpClient, productUriVersion1).GetAwaiter().GetResult();
            DisplayProducts(productsV2);
            Console.ReadLine();

            var categories = GetListDataAsync<CategoryDto>(httpClient, categoryUri).GetAwaiter().GetResult();
            DisplayCategories(categories);
            Console.ReadLine();


            Console.ReadKey();
        }


        private static HttpClient CreateHttpClient(string baseUri)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUri)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private static async Task<Uri> CreateAsync<T>(HttpClient httpClient, string uri, T item)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(uri, item);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        private static async Task<List<T>> GetListDataAsync<T>(HttpClient httpClient, string uri)
        {
            List<T> result = null;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<List<T>>();
            }

            return result;
        }

        private static void DisplayProducts(IEnumerable<ProductDto> products)
        {
            Console.WriteLine("Products: ");
            foreach (var product in products)
            {
                Console.WriteLine($"ProductId = {product.Id}, ProductName = {product.Name}");
            }
        }

        private static void DisplayCategories(IEnumerable<CategoryDto> categories)
        {
            Console.WriteLine("Categories: ");
            foreach (var category in categories)
            {
                Console.WriteLine($"CategoryId = {category.Id}, CategoryName = {category.Name}");
            }
        }
    }
}

using Microsoft.Extensions.Configuration;
using NorthwindSystem.BLL.Interface;
using System;

namespace NorthwindSystem.BLL
{
    public class BLLConfiguration : ILocalConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly string _maxProductNumber = "ProductsMaxNumber";

        public BLLConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetMaxProductsNumber()
        {
            return _configuration.GetValue<int>(_maxProductNumber);
        }
    }
}

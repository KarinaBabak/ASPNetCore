using System.Collections.Generic;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.Models
{
    public class CreateUpdateProductViewModel
    {
        public ProductDto Product { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<SupplierDto> Suppliers { get; set; }
    }
}

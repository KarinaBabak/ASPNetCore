using System.Collections.Generic;
using System.Threading.Tasks;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.BLL.Interface
{
    public interface IProductService
    {
        Task<int> Add(ProductDto model);
        Task Update(ProductDto model);
        Task Delete(ProductDto model);
        Task<ProductDto> GetById(int modelId);
        Task<IEnumerable<ProductDto>> GetAll();
    }
}

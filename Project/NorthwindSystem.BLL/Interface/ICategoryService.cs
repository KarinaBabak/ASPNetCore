using System.Collections.Generic;
using System.Threading.Tasks;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.BLL.Interface
{
    public interface ICategoryService
    {
        Task<int> Add(CategoryDto model);
        Task Update(CategoryDto model);
        Task Delete(CategoryDto model);
        Task<CategoryDto> GetById(int modelId);
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<byte[]> GetImage(int categoryId);
    }
}

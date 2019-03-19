using System.Collections.Generic;
using System.Threading.Tasks;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.BLL.Interface
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetById(int modelId);
        Task<IEnumerable<SupplierDto>> GetAll();
    }
}

using NorthwindSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CategoryDAOEntity = NorthwindSystem.Data.Entities.Category;

namespace NorthwindSystem.Persistence.Interface
{
    public interface ICategoryRepository : IRepository<CategoryDAOEntity>
    {
        Task<byte[]> GetImage(int categoryId);
        Task UpdateImage(int categoryId, byte[] updatedImage);
    }
}

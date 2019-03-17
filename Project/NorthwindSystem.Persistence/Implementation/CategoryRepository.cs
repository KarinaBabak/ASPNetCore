using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindSystem.Persistence.Interface;
using CategoryDAOEntity = NorthwindSystem.Data.Entities.Category;
using NorthwindSystem.Data;

namespace NorthwindSystem.Persistence.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NorthwindSystemContext _dbContext;

        public CategoryRepository(NorthwindSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(CategoryDAOEntity entity)
        {
            var resultEntity = await _dbContext.Set<CategoryDAOEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return resultEntity.Entity.CategoryId;
        }

        public async Task Delete(CategoryDAOEntity entity)
        {
            var category = _dbContext.Set<CategoryDAOEntity>().Where(c => c.CategoryId == entity.CategoryId).FirstOrDefault();
            if (category != null)
            {
                _dbContext.Set<CategoryDAOEntity>().Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryDAOEntity>> GetAll()
        {
            return await _dbContext.Set<CategoryDAOEntity>().ToListAsync();
        }

        public async Task<CategoryDAOEntity> GetById(int entityId)
        {
            return await _dbContext.Set<CategoryDAOEntity>().Where(a => a.CategoryId == entityId).FirstOrDefaultAsync();
        }

        public async Task Update(CategoryDAOEntity entity)
        {
            var category = _dbContext.Set<CategoryDAOEntity>().Where(a => a.CategoryId == entity.CategoryId).FirstOrDefault();
            if (category != null)
            {
                _dbContext.Set<CategoryDAOEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

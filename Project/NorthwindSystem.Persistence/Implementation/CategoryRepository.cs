using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindSystem.Persistence.Interface;
using CategoryDAOEntity = NorthwindSystem.Data.Entities.Category;
using NorthwindSystem.Data;
using System;

namespace NorthwindSystem.Persistence.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NorthwindSystemContext _dbContext;
        private const int GarbageBytesNumber = 78;

        public CategoryRepository(NorthwindSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(CategoryDAOEntity entity)
        {
            var resultEntity = await _dbContext.Categories.AddAsync(entity);
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
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<CategoryDAOEntity> GetById(int entityId)
        {
            return await _dbContext.Categories.Where(a => a.CategoryId == entityId).FirstOrDefaultAsync();
        }

        public async Task Update(CategoryDAOEntity entity)
        {
            var category = _dbContext.Categories.Where(a => a.CategoryId == entity.CategoryId).FirstOrDefault();
            if (category != null)
            {
                _dbContext.Set<CategoryDAOEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetImage(int categoryId)
        {
            var category = await GetById(categoryId);
            return GetCorrectImage(category?.Picture);
        }

        private byte[] GetCorrectImage(byte[] image)
        {
            return image?.Skip(GarbageBytesNumber).ToArray();
        }

        public async Task UpdateImage(int categoryId, byte[] updatedImage)
        {
            var updatedSecuredImage = GenerateGarbageData().Concat(updatedImage);
            var category = await GetById(categoryId);
            if (category == null)
            {
                return;
            }
            category.Picture = updatedSecuredImage.ToArray();
            _dbContext.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        private byte[] GenerateGarbageData(int size = GarbageBytesNumber)
        {
            var result = new byte[GarbageBytesNumber];
            new Random().NextBytes(result);

            return result;
        }
    }
}

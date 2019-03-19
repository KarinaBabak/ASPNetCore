using System;
using NorthwindSystem.Persistence.Interface;
using System.Collections.Generic;

using ProductDAOEntity = NorthwindSystem.Data.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using NorthwindSystem.Data;


namespace NorthwindSystem.Persistence.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthwindSystemContext _dbContext;

        public ProductRepository(NorthwindSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(ProductDAOEntity entity)
        {
            var resultEntity = await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return resultEntity.Entity.ProductId;
        }


        public async Task Delete(ProductDAOEntity entity)
        {
            var product = _dbContext.Set<ProductDAOEntity>().Where(c => c.ProductId == entity.ProductId).FirstOrDefault();
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductDAOEntity>> GetAll()
        {
            return await _dbContext.Products.Include(p => p.Category).Include(p => p.Supplier).ToListAsync();
        }

        public async Task<ProductDAOEntity> GetById(int entityId)
        {
            return await _dbContext.Products.Where(a => a.ProductId == entityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDAOEntity>> GetNumberItems(int number)
        {
            return await _dbContext.Products.Include(p => p.Category).Include(p => p.Supplier).Take(number).ToListAsync();
        }

        public async Task Update(ProductDAOEntity entity)
        {
            if (entity != null)
            {
                _dbContext.Products.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

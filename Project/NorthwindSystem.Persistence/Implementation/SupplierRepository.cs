using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindSystem.Data;
using NorthwindSystem.Data.Entities;
using NorthwindSystem.Persistence.Interface;


namespace NorthwindSystem.Persistence.Implementation
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly NorthwindSystemContext _dbContext;

        public SupplierRepository(NorthwindSystemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Add(Supplier entity)
        {
            var resultEntity = await _dbContext.Suppliers.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return resultEntity.Entity.SupplierId;
        }

        public async Task Delete(Supplier entity)
        {
            var product = _dbContext.Set<Supplier>().Where(c => c.SupplierId == entity.SupplierId).FirstOrDefault();
            if (product != null)
            {
                _dbContext.Suppliers.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await _dbContext.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetById(int entityId)
        {
            return await _dbContext.Suppliers.Where(a => a.SupplierId == entityId).FirstOrDefaultAsync();
        }

        public async Task Update(Supplier entity)
        {
            if (entity != null)
            {
                _dbContext.Suppliers.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

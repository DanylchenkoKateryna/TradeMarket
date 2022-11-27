using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly TradeMarketDbContext context;
        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(ProductCategory entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(ProductCategory entity)
        {
             context.ProductCategories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
             context.ProductCategories.RemoveRange(context.ProductCategories.Where(x => x.Id == id));
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await context.ProductCategories.AsNoTracking().ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await context.ProductCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(ProductCategory entity)
        {
            context.SaveChangesAsync();
        }
    }
}

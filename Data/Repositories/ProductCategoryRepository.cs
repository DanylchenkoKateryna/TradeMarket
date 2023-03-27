using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductCategoryRepository :IProductCategoryRepository
    {
        private readonly TradeMarketDbContext _context;

        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.AsNoTracking().Include(x=>x.Products).ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.AsNoTracking()
                .Include(x=>x.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(ProductCategory entity)
        {
            _context.ProductCategories.Update(entity);
            _context.SaveChanges();
        }

        public void Add(ProductCategory entity) => _context.ProductCategories.Add(entity);

        public void Delete(ProductCategory entity) => _context.ProductCategories.Remove(entity);
    }
}

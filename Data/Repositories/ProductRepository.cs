using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository :IProductRepository
    {
        private readonly TradeMarketDbContext _context;

        public ProductRepository(TradeMarketDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _context.Products.AsNoTracking().Include(x => x.ReceiptDetails).Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Products.AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.ReceiptDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
            _context.SaveChanges();
        }

        public void Add(Product entity) => _context.Products.Add(entity);

        public void Delete(Product entity) => _context.Products.Remove(entity);
    }
}

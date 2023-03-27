using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptRepository :IReceiptRepository
    {
        private readonly TradeMarketDbContext _context;

        public ReceiptRepository(TradeMarketDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await _context.Receipts
               .Include(x => x.Customer)
               .Include(x => x.Customer.Person)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category).ToListAsync();
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Receipts.AsNoTracking()
               .Include(x => x.Customer)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category)
               .FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Update(Receipt entity)
        {
            _context.Receipts.Update(entity);
            _context.SaveChanges();
        }
        public void Add(Receipt entity) => _context.Receipts.Add(entity);

        public void Delete(Receipt entity) => _context.Receipts.Remove(entity);
    }
}

using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptDetailRepository :IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext _context;

        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await _context.ReceiptDetails.AsNoTracking().Include(x => x.Product).Include(x => x.Receipt).Include(x => x.Product.Category).ToListAsync();
        }

        public void Update(ReceiptDetail entity)
        {
            _context.ReceiptDetails.Update(entity);
            _context.SaveChanges();
        }

        public void Add(ReceiptDetail entity) => _context.ReceiptDetails.Add(entity);

        public void Delete(ReceiptDetail entity) => _context.ReceiptDetails.Remove(entity);
    }
}

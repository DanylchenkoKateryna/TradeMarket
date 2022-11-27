using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext context;
        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(ReceiptDetail entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(ReceiptDetail entity)
        {
            context.ReceiptDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            context.ReceiptDetails.RemoveRange(context.ReceiptDetails.Where(x => x.Id == id));
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await context.ReceiptDetails.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await context.ReceiptDetails.AsNoTracking().Include(x => x.Product).Include(x => x.Receipt).Include(x => x.Product.Category).ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await context.ReceiptDetails.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(ReceiptDetail entity)
        {
            context.SaveChangesAsync();
        }
    }
}

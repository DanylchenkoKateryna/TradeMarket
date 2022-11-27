using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TradeMarketDbContext context;
        public CustomerRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Customer entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(Customer entity)
        {
            context.Customers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            context.Customers.RemoveRange(context.Customers.Where(x => x.Id == id));
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await context.Customers.AsNoTracking().Include(x => x.Person).Include(x => x.Receipts).ThenInclude(x => x.ReceiptDetails).ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await context.Customers.AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public void Update(Customer entity)
        {
           context.SaveChangesAsync();
        }
    }
}

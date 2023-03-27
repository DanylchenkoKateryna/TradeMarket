using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CustomerRepository :ICustomerRepository
    {
        private readonly TradeMarketDbContext _context;

        public CustomerRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await _context.Customers.AsNoTracking().Include(x => x.Person).Include(x => x.Receipts).ThenInclude(x => x.ReceiptDetails).ToListAsync();
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Customers
                .Include(x => x.Person)
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Customer entity)
        {
            _context.Customers.Update(entity);
            _context.SaveChanges();
        }
        
        public void Add(Customer entity)=>_context.Customers.Add(entity);
        
        public void Delete(Customer entity)=>_context.Customers.Remove(entity); 
    }
}

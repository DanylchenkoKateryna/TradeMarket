using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly TradeMarketDbContext context;
        public PersonRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Person entity)
        {
            await context.AddAsync(entity);
        }

        public void Delete(Person entity)
        {
             context.Persons.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            context.Persons.RemoveRange(context.Persons.Where(x => x.Id == id));
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await context.Persons.AsNoTracking().ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await context.Persons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void Update(Person entity)
        {
            await context.SaveChangesAsync();
        }
    }
}

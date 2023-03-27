using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class PersonRepository :IPersonRepository
    {
        private readonly TradeMarketDbContext _context;

        public PersonRepository(TradeMarketDbContext context)
        {
            _context = context;
        }

        public void Update(Person entity)
        {
            _context.Persons.Update(entity);
            _context.SaveChanges();
        }

        public void Add(Person entity) => _context.Persons.Add(entity);

        public void Delete(Person entity) => _context.Persons.Remove(entity);
    }
}

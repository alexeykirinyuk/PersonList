using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IQSoft.PersonList.Domain;
using LiteGuard;
using Microsoft.EntityFrameworkCore;

namespace IQSoft.PersonList.Server.Dal
{
    public sealed class PersonRepository : IPersonRepository
    {
        private readonly PersonListContext _context;

        public PersonRepository(PersonListContext context)
        {
            Guard.AgainstNullArgument(nameof(context), context);
            
            _context = context;
        }

        public Task<Person[]> GetAll()
        {
            return _context.Persons.ToArrayAsync();
        }

        public Task<Person> Get(int id)
        {
            return _context.Persons.FindAsync(id);
        }

        public async Task Add(Person person)
        {
            await _context.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Person person)
        {
            _context.Persons.Attach(person);
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);

            await _context.SaveChangesAsync();
        }
    }
}
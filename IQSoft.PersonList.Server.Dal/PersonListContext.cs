using IQSoft.PersonList.Domain;
using Microsoft.EntityFrameworkCore;

namespace IQSoft.PersonList.Server.Dal
{
    public sealed class PersonListContext : DbContext
    {
        public PersonListContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }
        
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .IsRequired();
            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .IsRequired();
            modelBuilder.Entity<Person>()
                .Property(p => p.Birthday)
                .IsRequired();
        }
    }
}
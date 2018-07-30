using System.Collections.Generic;
using System.Threading.Tasks;
using IQSoft.PersonList.Domain;

namespace IQSoft.PersonList.Server.Dal
{
    public interface IPersonRepository
    {
        Task<Person[]> GetAll();
        
        Task<Person> Get(int id);
        
        Task Add(Person person);
        
        Task Update(Person person);
        
        Task Delete(int id);
    }
}
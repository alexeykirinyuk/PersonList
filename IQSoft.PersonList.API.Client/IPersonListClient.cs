using IQSoft.PersonList.CQRS.Base;

namespace IQSoft.PersonList.API.Client
{
    public interface IPersonListClient
    {
        GetPersonListQueryResult Get(GetPersonListQuery query);
        
        GetPersonQueryResult Get(GetPersonQuery query);
        
        void Post(AddPersonCommand command);
        
        void Put(UpdatePersonCommand command);
        
        void Delete(DeletePersonCommand command);
    }
}
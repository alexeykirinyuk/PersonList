using System.Collections.Generic;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Domain;

namespace IQSoft.PersonList.CQRS.Base
{
    public sealed class GetPersonListQueryResult : QueryResultBase
    {
        public List<Person> Persons { get; set; }
    }
}
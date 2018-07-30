using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Server.Dal;
using LiteGuard;

namespace IQSoft.PersonList.CQRS.Queries.GetPerson
{
    public sealed class GetPersonQueryHandler : QueryHandlerBase<GetPersonQuery, GetPersonQueryResult>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonQueryHandler(IPersonRepository personRepository)
        {
            Guard.AgainstNullArgument(nameof(personRepository), personRepository);
            
            _personRepository = personRepository;
        }

        protected override Task Validate(GetPersonQuery request)
        {
            // TODO : validate ID
            
            return Task.CompletedTask;
        }

        protected override async Task<GetPersonQueryResult> Execute(GetPersonQuery query, CancellationToken cancellationToken)
        {
            return new GetPersonQueryResult()
            {
                Person = await _personRepository.Get(query.Id)
            };
        }
    }
}
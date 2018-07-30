using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Server.Dal;
using LiteGuard;
using MediatR;

namespace IQSoft.PersonList.CQRS.Queries.GetPersonList
{
    public sealed class GetPersonListQueryHandler : QueryHandlerBase<GetPersonListQuery, GetPersonListQueryResult>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonListQueryHandler(IPersonRepository personRepository)
        {
            Guard.AgainstNullArgument(nameof(personRepository), personRepository);
            
            _personRepository = personRepository;
        }

        protected override async Task<GetPersonListQueryResult> Execute(GetPersonListQuery request, CancellationToken cancellationToken)
        {
            return new GetPersonListQueryResult
            {
                Persons = (await _personRepository.GetAll()).ToList()
            };
        }
    }
}
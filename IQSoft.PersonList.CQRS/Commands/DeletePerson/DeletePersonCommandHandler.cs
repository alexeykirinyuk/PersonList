using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Domain;
using IQSoft.PersonList.Server.Dal;
using LiteGuard;

namespace IQSoft.PersonList.CQRS.Commands.DeletePerson
{
    public class DeletePersonCommandHandler : CommandHandlerBase<DeletePersonCommand>
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonCommandHandler(IPersonRepository personRepository)
        {
            Guard.AgainstNullArgument(nameof(personRepository), personRepository);
            
            _personRepository = personRepository;
        }

        protected override async Task<List<ValidationFailure>> Validate(DeletePersonCommand command)
        {
            var list = new List<ValidationFailure>();

            if (await _personRepository.Get(command.Id) == null)
            {
                list.Add(new ValidationFailure($"Person #{command.Id} not found."));    
            }

            return list;
        }

        protected override Task Execute(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            return _personRepository.Delete(command.Id);
        }
    }
}
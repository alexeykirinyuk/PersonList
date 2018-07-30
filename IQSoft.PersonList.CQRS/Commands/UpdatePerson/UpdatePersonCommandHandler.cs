using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Domain;
using IQSoft.PersonList.Server.Dal;
using LiteGuard;

namespace IQSoft.PersonList.CQRS.Commands.UpdatePerson
{
    public sealed class UpdatePersonCommandHandler : CommandHandlerBase<UpdatePersonCommand>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonCommandHandler(IPersonRepository personRepository)
        {
            Guard.AgainstNullArgument(nameof(personRepository), personRepository);
            
            _personRepository = personRepository;
        }

        protected override async Task<List<ValidationFailure>> Validate(UpdatePersonCommand command)
        {
            var list = new List<ValidationFailure>();

            if (await _personRepository.Get(command.Person.Id) == null)
            {
                list.Add(new ValidationFailure($"Person #{command.Person.Id} not found."));    
            }
            
            if (string.IsNullOrEmpty(command.Person.FirstName))
            {
                list.Add(new ValidationFailure("First name can't be null or empty."));
            }

            if (string.IsNullOrEmpty(command.Person.LastName))
            {
                list.Add(new ValidationFailure("Last name can't be null or empty."));
            }

            return list;
        }

        protected override Task Execute(UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            return _personRepository.Update(command.Person);
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.CQRS.Base;
using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Domain;
using IQSoft.PersonList.Server.Dal;
using LiteGuard;
using MediatR;

namespace IQSoft.PersonList.CQRS.Commands.AddPerson
{
    public sealed class AddPersonCommandHandler : CommandHandlerBase<AddPersonCommand>
    {
        private readonly IPersonRepository _personRepository;

        public AddPersonCommandHandler(IPersonRepository personRepository)
        {
            Guard.AgainstNullArgument(nameof(personRepository), personRepository);
            
            _personRepository = personRepository;
        }

        protected override Task<List<ValidationFailure>> Validate(AddPersonCommand request)
        {
            var list = new List<ValidationFailure>();
            
            if (string.IsNullOrEmpty(request.Person.FirstName))
            {
                list.Add(new ValidationFailure("First name can't be null or empty."));
            }

            if (string.IsNullOrEmpty(request.Person.LastName))
            {
                list.Add(new ValidationFailure("Last name can't be null or empty."));
            }
            
            return Task.FromResult(list);
        }

        protected override Task Execute(AddPersonCommand command, CancellationToken cancellationToken)
        {
            return _personRepository.Add(command.Person);
        }
    }
}
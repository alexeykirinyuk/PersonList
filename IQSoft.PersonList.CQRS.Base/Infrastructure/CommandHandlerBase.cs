using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IQSoft.PersonList.Domain;
using MediatR;

namespace IQSoft.PersonList.CQRS.Base.Infrastructure
{
    public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand>
        where TCommand : CommandBase
    {
        public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var failures = await Validate(request);
            if (failures.Any())
            {
                throw new ValidationException(failures.ToArray());
            }

            await Execute(request, cancellationToken);

            return Unit.Value;
        }

        protected virtual Task<List<ValidationFailure>> Validate(TCommand command)
        {
            return Task.FromResult(new List<ValidationFailure>());
        }

        protected abstract Task Execute(TCommand command, CancellationToken cancellationToken);
    }
}
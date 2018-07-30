using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace IQSoft.PersonList.CQRS.Base.Infrastructure
{
    public abstract class QueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IRequest<TResult>
        where TResult : QueryResultBase
    
    {
        public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
        {
            await Validate(request);
            
            return await Execute(request, cancellationToken);
        }

        protected virtual Task Validate(TQuery query)
        {
            return Task.CompletedTask;
        }

        protected abstract Task<TResult> Execute(TQuery query, CancellationToken cancellationToken);
    }
}
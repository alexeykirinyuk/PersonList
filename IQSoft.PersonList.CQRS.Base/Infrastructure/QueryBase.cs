using MediatR;

namespace IQSoft.PersonList.CQRS.Base.Infrastructure
{
    public abstract class QueryBase<TResult> : IRequest<TResult>
        where TResult : QueryResultBase
    {
    }
}
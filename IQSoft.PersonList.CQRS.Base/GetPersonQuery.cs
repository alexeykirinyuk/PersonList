using IQSoft.PersonList.CQRS.Base.Infrastructure;

namespace IQSoft.PersonList.CQRS.Base
{
    public sealed class GetPersonQuery : QueryBase<GetPersonQueryResult>
    {
        public int Id { get; set; }
    }
}
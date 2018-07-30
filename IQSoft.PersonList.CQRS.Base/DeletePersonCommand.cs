using IQSoft.PersonList.CQRS.Base.Infrastructure;

namespace IQSoft.PersonList.CQRS.Base
{
    public sealed class DeletePersonCommand : CommandBase
    {
        public int Id { get; set; }
    }
}
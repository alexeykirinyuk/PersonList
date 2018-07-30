using IQSoft.PersonList.CQRS.Base.Infrastructure;
using IQSoft.PersonList.Domain;

namespace IQSoft.PersonList.CQRS.Base
{
    public sealed class AddPersonCommand : CommandBase
    {
        public Person Person { get; set; }
    }
}
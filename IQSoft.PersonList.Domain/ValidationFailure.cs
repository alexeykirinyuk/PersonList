using LiteGuard;

namespace IQSoft.PersonList.Domain
{
    public class ValidationFailure
    {
        public string Message { get; }

        public ValidationFailure(string message)
        {
            Guard.AgainstNullArgument(nameof(message), message);
            
            Message = message;
        }
    }
}
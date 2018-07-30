using System;
using System.Collections;
using LiteGuard;

namespace IQSoft.PersonList.Domain
{
    public sealed class ErrorResponse
    {
        public ValidationFailure[] Failures { get; set; }

        public ValidationException AsException()
        {
            return new ValidationException(Failures);
        }
    }
    
    public sealed class ValidationException : Exception
    {
        public ValidationFailure[] Failures { get; }

        public ValidationException(ValidationFailure[] failures)
        {
            Guard.AgainstNullArgument(nameof(failures), failures);

            Failures = failures;
        }

        public ErrorResponse AsResponse()
        {
            return new ErrorResponse {Failures = Failures};
        }
    }
}
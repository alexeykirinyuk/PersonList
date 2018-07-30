using System.Collections.Generic;
using IQSoft.PersonList.Domain;

namespace IQSoft.PersonList.Web.Models
{
    public sealed class FormModel<T>
    {
        public T Data { get; set; }

        public ValidationException ValidationException { get; set; }
    }

    public static class FormModel
    {
        public static FormModel<T> FromModel<T>(T model, ValidationException exception = null)
        {
            return new FormModel<T>
            {
                Data = model,
                ValidationException = exception ?? new ValidationException(new ValidationFailure[0])
            };
        }
    }
}
using System;

namespace IQSoft.PersonList.Domain
{
    public sealed class Person : IWithId
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Patronymic { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}
using Codesanook.Examples.DotNetCore.Orm.NHExamples;
using System;

namespace Codesanook.Examples.DotNetCore.Models
{
    [Serializable]
    public class User : Entity, IAuditable
    {
        public User() { }
        public User(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetFullName() => $"{FirstName} {LastName}";
        public Profile Profile { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}

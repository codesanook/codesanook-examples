using Codesanook.Examples.DotNetCore.Orm.NHExamples;
using System;

namespace Codesanook.Examples.CSharp.Models
{
    [Serializable]
    public class User : IAuditable
    {
        public User(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetFullName() => $"{FirstName} {LastName}";
        public Profile Profile { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}

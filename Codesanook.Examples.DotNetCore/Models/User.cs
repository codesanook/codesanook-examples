using System;

namespace Codesanook.Examples.CSharp.Models
{
    [Serializable]
    public class User
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
    }
}
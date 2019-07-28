namespace Codesanook.Examples.CSharp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GetFullName() => $"{FirstName} {LastName}";
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        public int Id { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
    }
}
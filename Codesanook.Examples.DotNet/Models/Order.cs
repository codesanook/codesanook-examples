using System;

namespace Codesanook.Examples.DotNet.Models
{
    [Serializable]
    public class Order
    {
        private User user;

        public int Id { get; set; }
        public string Name { get; set; }

        public User User
        {
            get => user;
            set => user = value;
        }
    }
}

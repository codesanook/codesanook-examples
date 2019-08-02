using System.ComponentModel.DataAnnotations;

namespace Codesanook.Examples.AspNetMvc.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string IdCardNumber { get; set; }
    }
}
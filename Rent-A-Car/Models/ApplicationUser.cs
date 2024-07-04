using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public string PIN { get; set; }
    }
}

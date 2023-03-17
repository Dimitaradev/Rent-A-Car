using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rent_A_Car.Models;

namespace Rent_A_Car.Data
{
    public class Requests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CarID { get; set; }

        public virtual Cars Car { get; set; }

        [Required]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime RequestStart { get; set; }

        [Required]
        public DateTime RequestEnd { get; set; }
    }

}

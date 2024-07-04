using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
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

        public bool IsApproved { get; set; }
    }

}

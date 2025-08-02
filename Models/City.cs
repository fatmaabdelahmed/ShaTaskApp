using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaTaskApp.Models
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CityName { get; set; } = "";

        public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    }
}

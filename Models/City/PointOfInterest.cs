using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vega.Models.City {
    public class POI {

        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength (30)]
        public string Name { get; set; }

        [MaxLength (100)]
        public string Description { get; set; }

        [ForeignKey ("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }

    }
}
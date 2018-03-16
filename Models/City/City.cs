using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vega.Models.City {
    public class City {

        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength (30)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<POI> POIs { get; set; } = new List<POI> ();

    }
}
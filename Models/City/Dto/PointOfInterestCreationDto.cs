using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vega.Models.City.Dto {
    public class PointOfInterestCreationDto {

        [Required (ErrorMessage = "Cannot be empty")]
        [MaxLength (20)]
        public string Name { get; set; }

        [MaxLength (130)]
        public string Description { get; set; }
    }
}
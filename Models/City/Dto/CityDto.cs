using System.Collections.Generic;

namespace Vega.Models.City.Dto {
    public class CityDto {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterest {
            get {
                return 10;
            }
        }

        //Syntax C# 6.0
        public IEnumerable<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto> ();

    }
}
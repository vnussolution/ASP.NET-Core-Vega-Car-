using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Vega.Models.City;
using Vega.Models.City.Dto;
using Vega.Services.City;

namespace Vega.Controllers {

    [Route ("api/Cities")]
    public class CitiesController : Controller {

        private readonly ICityInfoRepository _cityInfoRepository;
        public CitiesController (ICityInfoRepository cityInfoRepository) {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities () {

            var cities = _cityInfoRepository.GetCities ();

            //use AutoMapper to replace manual mapping
            var results = AutoMapper.Mapper.Map<IEnumerable<CityWithoutPoiDto>> (cities);

            // var results = new List<CityWithoutPoiDto> ();

            // foreach (var city in cities) {
            //     results.Add (new CityWithoutPoiDto {
            //         Id = city.Id,
            //             Description = city.Description,
            //             Name = city.Name
            //     });
            // }

            return Ok (results);
        }

        [HttpGet ("{id}")]
        public IActionResult GetCities (int id, bool includePoi = false) {

            // var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault (c => c.Id == id);
            var city = _cityInfoRepository.GetCity (id, includePoi);

            if (city == null) return NotFound ();

            if (includePoi) {
                // var resultWithPoi = new CityDto {
                //     Id = city.Id,
                //     Name = city.Name,
                //     Description = city.Description
                // };
                // foreach (var poi in city.POIs) {
                //     resultWithPoi.PointsOfInterest.Add (new PointOfInterestDto {
                //         Id = poi.Id,
                //             Name = poi.Name,
                //             Description = poi.Description
                //     });
                // }

                //replace manual mapping with AutoMapper
                var resultWithPoi = AutoMapper.Mapper.Map<CityDto> (city);
                var poiList = AutoMapper.Mapper.Map<IEnumerable<PointOfInterestDto>> (city.POIs);
                resultWithPoi.PointsOfInterest = poiList;
                return Ok (resultWithPoi);
            }

            // var resultWithoutPoi = new CityWithoutPoiDto {
            //     Id = city.Id,
            //     Name = city.Name,
            //     Description = city.Description
            // };

            var resultWithoutPoi = AutoMapper.Mapper.Map<CityDto> (city);

            return Ok (resultWithoutPoi);
        }
    }
}
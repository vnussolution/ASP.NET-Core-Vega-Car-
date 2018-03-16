using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vega.Models.City;
using Vega.Models.City.Dto;
using Vega.Services.City;

namespace Vega.Controllers {

    [Route ("api/Cities")]
    public class PointsOfInterestController : Controller {

        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController (ICityInfoRepository cityInfoRepository, IMailService mailService, ILogger<PointsOfInterestController> logger) {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet ("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest (int cityId) {
            if (!_cityInfoRepository.CityExists (cityId)) {
                _logger.LogInformation ($"================>City with id {cityId} not found");
                return NotFound ();
            }

            var POIs = _cityInfoRepository.GetPOIsForCity (cityId);
            // var POIsResult = new List<PointOfInterestDto> ();
            // foreach (var poi in POIs) {
            //     POIsResult.Add (new PointOfInterestDto {
            //         Id = poi.Id,
            //             Name = poi.Name,
            //             Description = poi.Description
            //     });
            // }

            var POIsResult = AutoMapper.Mapper.Map<IEnumerable<PointOfInterestDto>> (POIs);

            return Ok (POIsResult);
        }

        [HttpGet ("{cityId}/pointsofinterest/{id}", Name = "GetPoi")]
        public IActionResult GetCities (int cityId, int id) {

            var POI = _cityInfoRepository.GetPOIForCity (cityId, id);

            if (POI == null) return NotFound ();

            // var result = new PointOfInterestDto {
            //     Id = POI.Id,
            //     Name = POI.Name,
            //     Description = POI.Description
            // };

            var result = AutoMapper.Mapper.Map<PointOfInterestDto> (POI);

            return Ok (result);
        }

        [HttpPost ("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest (int cityId, [FromBody] PointOfInterestCreationDto poiDto) {

            //check if dto is empty
            if (poiDto == null) return BadRequest ();

            // custom validation
            if (poiDto.Description == poiDto.Name)
                ModelState.AddModelError ("Description", "Description and name can't be the same");

            if (!_cityInfoRepository.CityExists (cityId)) return NotFound ();

            // validation: check against dto annotations
            if (!ModelState.IsValid) return BadRequest (ModelState);

            var poi = AutoMapper.Mapper.Map<POI> (poiDto);
            _cityInfoRepository.AddPoiForCity (cityId, poi);
            if (!_cityInfoRepository.Save ()) {
                return StatusCode (500, "A problem happened while handleing your request");
            }

            //map back to return
            var createdPoi = AutoMapper.Mapper.Map<PointOfInterestDto> (poi);
            return CreatedAtRoute ("GetPoi", new { cityId = cityId, id = createdPoi.Id }, createdPoi);
        }

        [HttpPut ("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest (int cityId, int id, [FromBody] PointOfInterestCreationDto poiDto) {

            #region do validation checks
            if (poiDto == null) return BadRequest ();

            if (poiDto.Description == poiDto.Name)
                ModelState.AddModelError ("Description", "can't be the same");

            if (!ModelState.IsValid) return BadRequest (ModelState);

            if (!_cityInfoRepository.CityExists (cityId)) return NotFound ();

            var poi = _cityInfoRepository.GetPOIForCity (cityId, id);

            if (poi == null) return NotFound ();

            #endregion

            // poi.Name = poiDto.Name;
            // poi.Description = poiDto.Description;

            //Use AutoMapper to update properties , from source ...to destination
            AutoMapper.Mapper.Map (poiDto, poi);

            if (!_cityInfoRepository.Save ()) {
                return StatusCode (500, "A problem happened while handling Updating");
            }
            return NoContent (); // 204
        }

        [HttpPatch ("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePoi (int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestUpdateDto> patchDoc) {

            if (patchDoc == null) return BadRequest ();

            if (!_cityInfoRepository.CityExists (cityId)) return NotFound ();

            var poi = _cityInfoRepository.GetPOIForCity (cityId, id);
            if (poi == null) return NotFound ();

            //first, map dto to dto (map dto from the view to new dto which is going to map to EF model   )
            var poiUpdateDto = AutoMapper.Mapper.Map<PointOfInterestUpdateDto> (poi);

            //patch patchDoc to poiUpdateDto
            patchDoc.ApplyTo (poiUpdateDto, ModelState);
            // return error if it violates
            if (!ModelState.IsValid) return BadRequest (ModelState);

            // handle when input violates dto annotations
            TryValidateModel (poiUpdateDto);

            //custom validation
            if (poiUpdateDto.Name == poiUpdateDto.Description) ModelState.AddModelError ("Description", "Description and name can't be the same");
            if (!ModelState.IsValid) return BadRequest (ModelState);

            //second ,Map dto to model
            AutoMapper.Mapper.Map (poiUpdateDto, poi);

            if (!_cityInfoRepository.Save ()) {
                return StatusCode (500, "A problem happened while handling Updating");
            }
            return NoContent ();

        }

        [HttpDelete ("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest (int cityId, int id) {

            var city = _cityInfoRepository.GetCity (cityId, true);
            if (city == null) return NotFound ();

            var poiFromStore = _cityInfoRepository.GetPOIForCity (cityId, id);

            if (poiFromStore == null) return NotFound ();

            // city.POIs.Remove (poiFromStore);

            _cityInfoRepository.DeletePoi (poiFromStore);

            if (!_cityInfoRepository.Save ()) {
                return StatusCode (500, "A problem happened while handling Updating");
            }

            _mailService.Send ("=============================>>>>>>>>>>>Point of intereste deleted", $"POI {poiFromStore.Name} with id {poiFromStore.Id} was deleted");

            return NoContent ();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vega.Models;
using Vega.Models.City;

namespace Vega.Services.City {
    public class CityInfoRepository : ICityInfoRepository {

        private AppDbContext _context;

        public CityInfoRepository (AppDbContext context) {
            _context = context;
        }
        public IEnumerable<Models.City.City> GetCities () {
            return _context.Cities.OrderBy (c => c.Name).ToList ();
        }

        public Models.City.City GetCity (int cityId, bool includePOI = false) {
            if (includePOI) {
                return _context.Cities.Where (c => c.Id == cityId).Include (p => p.POIs).FirstOrDefault ();
            }
            return _context.Cities.Where (c => c.Id == cityId).FirstOrDefault ();
        }

        public POI GetPOIForCity (int cityId, int poiId) {
            return _context.POI.Where (p => p.Id == poiId && p.CityId == cityId).FirstOrDefault ();
        }

        public IEnumerable<POI> GetPOIsForCity (int cityId) {
            return _context.POI.Where (c => c.CityId == cityId).ToList ();
        }

        public bool CityExists (int id) {
            return _context.Cities.Any (c => c.Id == id);
        }

        public void AddPoiForCity (int cityId, POI poi) {
            var city = GetCity (cityId);
            city.POIs.Add (poi);
        }

        public bool Save () {
            return _context.SaveChanges () > 0;
        }

        public void DeletePoi (POI poi) {
            _context.POI.Remove (poi);
        }
    }
}
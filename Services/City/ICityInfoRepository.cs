using System.Collections.Generic;
using Vega.Models.City;
namespace Vega.Services.City {

    public interface ICityInfoRepository {
        IEnumerable<Vega.Models.City.City> GetCities ();

        Vega.Models.City.City GetCity (int cityId, bool includePOI);

        IEnumerable<POI> GetPOIsForCity (int cityId);
        POI GetPOIForCity (int cityId, int poiId);
        bool CityExists (int cityId);
        bool Save ();
        void AddPoiForCity (int cityId, POI poi);
        void DeletePoi (POI pOI);

    }
}
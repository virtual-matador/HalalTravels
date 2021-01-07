using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Zones;

namespace Nop.Services.Zones
{
    public partial interface ICityService
    {
        #region Cities

        /// <summary>
        /// Delete a city
        /// </summary>
        /// <param name="city">City</param>
        void DeleteCity(City city);

        /// <summary>
        /// Delete cities
        /// </summary>
        /// <param name="cities">Cities</param>
        void DeleteCities(IList<City> cities);

        /// <summary>
        /// Gets city
        /// </summary>
        /// <param name="cityId">City identifier</param>
        /// <returns>City</returns>
        City GetCityById(int cityId);

        /// <summary>
        /// Gets cities by identifier
        /// </summary>
        /// <param name="cityIds">City identifiers</param>
        /// <returns>Cities</returns>
        IList<City> GetCitiesByIds(int[] cityIds);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cities</returns>
        IList<City> GetAllCities(bool showHidden = false);

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="cityName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cities</returns>
        IPagedList<City> GetAllCities(string cityName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a city
        /// </summary>
        /// <param name="city">City</param>
        void InsertCity(City city);

        /// <summary>
        /// Updates the city
        /// </summary>
        /// <param name="city">City</param>
        void UpdateCity(City city);

        /// <summary>
        /// Updates the cities
        /// </summary>
        /// <param name="cities">City</param>
        void UpdateCities(IList<City> cities);

        #endregion

        IList<City> GetCitiesByCountryId(int countryId, bool showHidden = false);

        IPagedList<City> GetCitiesByCountryId(int countryId, string cityName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        
        IList<City> GetCitiesByProvinceId(int provinceId, bool showHidden = false);

        IPagedList<City> GetCitiesByProvinceId(int provinceId, string cityName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        
        IList<City> GetCitiesByCountyId(int countyId, bool showHidden = false);
        
        IPagedList<City> GetCitiesByCountyId(int countyId, string cityName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}

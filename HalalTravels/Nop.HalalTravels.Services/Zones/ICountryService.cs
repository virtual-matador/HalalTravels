using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Zones;

namespace Nop.Services.Zones
{
    public partial interface ICountryService
    {
        #region Countries

        /// <summary>
        /// Delete a country
        /// </summary>
        /// <param name="country">Country</param>
        void DeleteCountry(Country country);

        /// <summary>
        /// Delete countries
        /// </summary>
        /// <param name="countries">Countries</param>
        void DeleteCountries(IList<Country> countries);

        /// <summary>
        /// Gets country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        Country GetCountryById(int countryId);

        /// <summary>
        /// Gets countries by identifier
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        IList<Country> GetCountriesByIds(int[] countryIds);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        IList<Country> GetAllCountries(bool showHidden = false);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="countryName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        IPagedList<Country> GetAllCountries(string countryName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        void InsertCountry(Country country);

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        void UpdateCountry(Country country);

        /// <summary>
        /// Updates the countries
        /// </summary>
        /// <param name="countries">Country</param>
        void UpdateCountries(IList<Country> countries);

        #endregion
    }
}

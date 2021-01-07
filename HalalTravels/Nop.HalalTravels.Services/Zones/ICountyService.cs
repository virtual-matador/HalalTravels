using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Zones;

namespace Nop.Services.Zones
{
    public interface ICountyService
    {
        #region Counties

        /// <summary>
        /// Delete a county
        /// </summary>
        /// <param name="county">County</param>
        void DeleteCounty(County county);

        /// <summary>
        /// Delete counties
        /// </summary>
        /// <param name="counties">Counties</param>
        void DeleteCounties(IList<County> counties);

        /// <summary>
        /// Gets county
        /// </summary>
        /// <param name="countyId">County identifier</param>
        /// <returns>County</returns>
        County GetCountyById(int countyId);

        /// <summary>
        /// Gets counties by identifier
        /// </summary>
        /// <param name="countyIds">County identifiers</param>
        /// <returns>Counties</returns>
        IList<County> GetCountiesByIds(int[] countyIds);

        /// <summary>
        /// Gets all counties
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Counties</returns>
        IList<County> GetAllCounties(bool showHidden = false);

        /// <summary>
        /// Gets all counties
        /// </summary>
        /// <param name="countyName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Counties</returns>
        IPagedList<County> GetAllCounties(string countyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a county
        /// </summary>
        /// <param name="county">County</param>
        void InsertCounty(County county);

        /// <summary>
        /// Updates the county
        /// </summary>
        /// <param name="county">County</param>
        void UpdateCounty(County county);

        /// <summary>
        /// Updates the counties
        /// </summary>
        /// <param name="counties">County</param>
        void UpdateCounties(IList<County> counties);

        #endregion

        IList<County> GetCountiesByCountryId(int countryId, bool showHidden = false);

        IPagedList<County> GetCountiesByCountryId(int countryId, string countyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        
        IList<County> GetCountiesByProvinceId(int provinceId, bool showHidden = false);

        IPagedList<County> GetCountiesByProvinceId(int provinceId, string countyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}
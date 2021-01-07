using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Zones;

namespace Nop.Services.Zones
{
    public partial interface IProvinceService
    {
        #region Provinces

        /// <summary>
        /// Delete a province
        /// </summary>
        /// <param name="province">Province</param>
        void DeleteProvince(Province province);

        /// <summary>
        /// Delete provinces
        /// </summary>
        /// <param name="provinces">Provinces</param>
        void DeleteProvinces(IList<Province> provinces);

        /// <summary>
        /// Gets province
        /// </summary>
        /// <param name="provinceId">Province identifier</param>
        /// <returns>Province</returns>
        Province GetProvinceById(int provinceId);

        /// <summary>
        /// Gets provinces by identifier
        /// </summary>
        /// <param name="provinceIds">Province identifiers</param>
        /// <returns>Provinces</returns>
        IList<Province> GetProvincesByIds(int[] provinceIds);

        /// <summary>
        /// Gets all provinces
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Provinces</returns>
        IList<Province> GetAllProvinces(bool showHidden = false);

        /// <summary>
        /// Gets all provinces
        /// </summary>
        /// <param name="provinceName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Provinces</returns>
        IPagedList<Province> GetAllProvinces(string provinceName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a province
        /// </summary>
        /// <param name="province">Province</param>
        void InsertProvince(Province province);

        /// <summary>
        /// Updates the province
        /// </summary>
        /// <param name="province">Province</param>
        void UpdateProvince(Province province);

        /// <summary>
        /// Updates the provinces
        /// </summary>
        /// <param name="provinces">Province</param>
        void UpdateProvinces(IList<Province> provinces);

        #endregion

        IList<Province> GetProvincesByCountryId(int countryId, bool showHidden = false);

        IPagedList<Province> GetProvincesByCountryId(int countryId, string provinceName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}

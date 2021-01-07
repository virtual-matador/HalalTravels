using System.Collections;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// HotelType service
    /// </summary>
    public partial interface IHotelTypeService
    {
        #region HotelTypes

        /// <summary>
        /// Delete a hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        void DeleteHotelType(HotelType hotelType);

        /// <summary>
        /// Delete hotelTypes
        /// </summary>
        /// <param name="hotelTypes">HotelTypes</param>
        void DeleteHotelTypes(IList<HotelType> hotelTypes);

        /// <summary>
        /// Gets hotelType
        /// </summary>
        /// <param name="hotelTypeId">HotelType identifier</param>
        /// <returns>HotelType</returns>
        HotelType GetHotelTypeById(int hotelTypeId);

        /// <summary>
        /// Gets hotelTypes by identifier
        /// </summary>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <returns>HotelTypes</returns>
        IList<HotelType> GetHotelTypesByIds(int[] hotelTypeIds);

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelTypes</returns>
        IList<HotelType> GetAllHotelTypes(bool showHidden = false);

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="hotelTypeName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelTypes</returns>
        IPagedList<HotelType> GetAllHotelTypes(string hotelTypeName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<HotelType> SearchHotelTypes(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        void InsertHotelType(HotelType hotelType);

        /// <summary>
        /// Updates the hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        void UpdateHotelType(HotelType hotelType);

        /// <summary>
        /// Updates the hotelTypes
        /// </summary>
        /// <param name="hotelTypes">HotelType</param>
        void UpdateHotelTypes(IList<HotelType> hotelTypes);

        #endregion
    }
}

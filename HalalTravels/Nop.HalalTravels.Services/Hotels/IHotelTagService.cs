using System.Collections.Generic;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    public partial interface IHotelTagService
    {
        /// <summary>
        /// Delete a hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        void DeleteHotelTag(HotelTag hotelTag);

        /// <summary>
        /// Delete hotel tags
        /// </summary>
        /// <param name="hotelTags">Hotel tags</param>
        void DeleteHotelTags(IList<HotelTag> hotelTags);

        /// <summary>
        /// Gets hotel tags
        /// </summary>
        /// <param name="hotelTagIds">Hotel tags identifiers</param>
        /// <returns>Hotel tags</returns>
        IList<HotelTag> GetHotelTagsByIds(int[] hotelTagIds);

        /// <summary>
        /// Indicates whether a hotel tag exists
        /// </summary>
        /// <param name="hotel">Hotel</param>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <returns>Result</returns>
        bool HotelTagExists(Hotel hotel, int hotelTagId);

        /// <summary>
        /// Gets all hotel tags
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Hotel tags</returns>
        IList<HotelTag> GetAllHotelTags(string tagName = null);

        /// <summary>
        /// Gets all hotel tags by hotel identifier
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <returns>Hotel tags</returns>
        IList<HotelTag> GetAllHotelTagsByHotelId(int hotelId);

        /// <summary>
        /// Gets hotel tag
        /// </summary>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <returns>Hotel tag</returns>
        HotelTag GetHotelTagById(int hotelTagId);

        /// <summary>
        /// Gets hotel tag by name
        /// </summary>
        /// <param name="name">Hotel tag name</param>
        /// <returns>Hotel tag</returns>
        HotelTag GetHotelTagByName(string name);

        /// <summary>
        /// Inserts a hotel-hotel tag mapping
        /// </summary>
        /// <param name="tagMapping">Hotel-hotel tag mapping</param>
        void InsertHotelTagMapping(HotelTagMapping tagMapping);

        /// <summary>
        /// Inserts a hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        void InsertHotelTag(HotelTag hotelTag);

        /// <summary>
        /// Updates the hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        void UpdateHotelTag(HotelTag hotelTag);

        /// <summary>
        /// Get number of hotels
        /// </summary>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Number of hotels</returns>
        int GetHotelCount(int hotelTagId, int storeId, bool showHidden = false);

        /// <summary>
        /// Update hotel tags
        /// </summary>
        /// <param name="hotel">Hotel for update</param>
        /// <param name="hotelTags">Hotel tags</param>
        void UpdateHotelTags(Hotel hotel, string[] hotelTags);
    }
}

using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the hotel type model factory
    /// </summary>
    public partial interface IHotelTypeModelFactory
    {
        /// <summary>
        /// Prepare hotel type search model
        /// </summary>
        /// <param name="searchModel">HotelType search model</param>
        /// <returns>HotelType search model</returns>
        HotelTypeSearchModel PrepareHotelTypeSearchModel(HotelTypeSearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel type list model
        /// </summary>
        /// <param name="searchModel">HotelType search model</param>
        /// <returns>HotelType list model</returns>
        HotelTypeListModel PrepareHotelTypeListModel(HotelTypeSearchModel searchModel);

        /// <summary>
        /// Prepare hotel type model
        /// </summary>
        /// <param name="model">HotelType model</param>
        /// <param name="hotelType">HotelType</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>HotelType model</returns>
        HotelTypeModel PrepareHotelTypeModel(HotelTypeModel model, HotelType hotelType, bool excludeProperties = false);
    }
}

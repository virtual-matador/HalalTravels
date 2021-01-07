using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the hotel model factory
    /// </summary>
    public partial interface IHotelModelFactory
    {
        /// <summary>
        /// Prepare hotel search model
        /// </summary>
        /// <param name="searchModel">Hotel search model</param>
        /// <returns>Hotel search model</returns>
        HotelSearchModel PrepareHotelSearchModel(HotelSearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel list model
        /// </summary>
        /// <param name="searchModel">Hotel search model</param>
        /// <returns>Hotel list model</returns>
        HotelListModel PrepareHotelListModel(HotelSearchModel searchModel);

        /// <summary>
        /// Prepare hotel model
        /// </summary>
        /// <param name="model">Hotel model</param>
        /// <param name="hotel">Hotel</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Hotel model</returns>
        HotelModel PrepareHotelModel(HotelModel model, Hotel hotel, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged hotel picture list model
        /// </summary>
        /// <param name="searchModel">Hotel picture search model</param>
        /// <param name="hotel">Hotel</param>
        /// <returns>Hotel picture list model</returns>
        HotelPictureListModel PrepareHotelPictureListModel(HotelPictureSearchModel searchModel, Hotel hotel);

        RelatedHotelListModel PrepareRelatedHotelListModel(RelatedHotelSearchModel searchModel, Hotel hotel);

        /// <summary>
        /// Prepare related hotel search model to add to the hotel
        /// </summary>
        /// <param name="searchModel">Related hotel search model to add to the hotel</param>
        /// <returns>Related hotel search model to add to the hotel</returns>
        AddRelatedHotelSearchModel PrepareAddRelatedHotelSearchModel(
            AddRelatedHotelSearchModel searchModel);

        /// <summary>
        /// Prepare paged related hotel list model to add to the hotel
        /// </summary>
        /// <param name="searchModel">Related hotel search model to add to the hotel</param>
        /// <returns>Related hotel list model to add to the hotel</returns>
        AddRelatedHotelListModel PrepareAddRelatedHotelListModel(AddRelatedHotelSearchModel searchModel);
        
        /// <summary>
        /// Prepare hotel tag search model
        /// </summary>
        /// <param name="searchModel">Hotel tag search model</param>
        /// <returns>Hotel tag search model</returns>
        HotelTagSearchModel PrepareHotelTagSearchModel(HotelTagSearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel tag list model
        /// </summary>
        /// <param name="searchModel">Hotel tag search model</param>
        /// <returns>Hotel tag list model</returns>
        HotelTagListModel PrepareHotelTagListModel(HotelTagSearchModel searchModel);

        /// <summary>
        /// Prepare hotel tag model
        /// </summary>
        /// <param name="model">Hotel tag model</param>
        /// <param name="hotelTag">Hotel tag</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Hotel tag model</returns>
        HotelTagModel PrepareHotelTagModel(HotelTagModel model, HotelTag hotelTag, bool excludeProperties = false);
        
        HotelContactListModel PrepareHotelContactListModel(HotelContactSearchModel searchModel, Hotel hotel);

        HotelContactModel PrepareHotelContactModel(HotelContactModel model, Hotel hotel, HotelContact hotelContact, bool excludeProperties = false);

        HotelContactSearchModel PrepareHotelContactSearchModel(HotelContactSearchModel searchModel, Hotel hotel);
        HotelContractDocumentSearchModel PrepareContractDocumentSearchModel(HotelContractDocumentSearchModel searchModel, Hotel hotel);
        
        HotelContractDocumentListModel PrepareContractDocumentListModel(HotelContractDocumentSearchModel searchModel, Hotel hotel);
        
        HotelSelectorModel PrepareHotelSelectorModel();
    }
}

using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    public partial interface IHotelPropertyService
    {
        #region Property types

        IList<PropertyType> GetAllPropertyTypes(bool showHidden = false);
        
        IList<PropertyType> GetAllPropertyTypesForHotel(bool showHidden = false);

        #endregion
        
        #region Property headers

        IList<PropertyHeader> GetAllPropertyHeaders(bool showHidden = false);
        
        IList<PropertyHeader> GetAllPropertyHeadersForHotel(bool showHidden = false);
        
        IList<PropertyHeader> GetPropertyHeadersByPropertyTypeid(int propertyTypeId, bool showHidden = false);

        #endregion
        
        #region Property details

        IList<PropertyDetail> GetPropertyDetailsByHeaderId(int propertyHeaderId, bool showHidden = false);

        #endregion
        
        #region Hotel properties

        void DeleteHotelProperty(HotelProperty hotelProperty);

        void DeleteHotelProperties(IList<HotelProperty> hotelProperties);

        HotelProperty GetHotelPropertyById(int hotelPropertyId);

        IList<HotelProperty> GetHotelPropertiesByIds(int[] hotelPropertyIds);

        IList<HotelProperty> GetAllHotelProperties(bool showHidden = false);
        
        IList<HotelProperty> GetAllHotelPropertiesByHotelId(int hotelId, bool showHidden = false);

        void InsertHotelProperty(HotelProperty hotelProperty);

        void UpdateHotelProperty(HotelProperty hotelProperty);

        void UpdateHotelProperties(IList<HotelProperty> hotelProperties);

        #endregion
    }
}
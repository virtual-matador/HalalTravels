using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public class HotelAmenitiesModel : BaseNopEntityModel
    {
        #region Properties

        public int HotelId { get; set; }
        
        public int CityId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelCityMapping.Fields.CityName")]
        public string CityName { get; set; }

        #endregion
    }
}
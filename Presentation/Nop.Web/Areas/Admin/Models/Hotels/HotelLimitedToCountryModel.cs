using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class HotelLimitedToCountryModel : BaseNopEntityModel
    {
        #region Properties

        public int HotelId { get; set; }
        
        public int CountryId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotels.HotelLimitedToCountry.Fields.CountryName")]
        public string CountryName { get; set; }

        #endregion
    }
}
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class CategoryHotelModel : BaseNopEntityModel
    {
        #region Properties

        public int CategoryId { get; set; }

        public int HotelId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Categories.Hotels.Fields.Hotel")]
        public string HotelName { get; set; }

        #endregion
    }
}

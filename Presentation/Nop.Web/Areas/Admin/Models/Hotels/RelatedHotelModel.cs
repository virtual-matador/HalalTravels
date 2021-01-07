using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a related hotel model
    /// </summary>
    public partial class RelatedHotelModel : BaseNopEntityModel
    {
        #region Properties

        public int RelatedHotelId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotels.RelatedHotels.Fields.Hotel")]
        public string RelatedHotelName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotels.RelatedHotels.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a related hotel search model
    /// </summary>
    public partial class RelatedHotelSearchModel : BaseSearchModel
    {
        #region Properties

        public int HotelId { get; set; }

        #endregion
    }
}
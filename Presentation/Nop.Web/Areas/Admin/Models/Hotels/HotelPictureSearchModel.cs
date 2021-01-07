using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel picture search model
    /// </summary>
    public partial class HotelPictureSearchModel : BaseSearchModel
    {
        #region Properties

        public int HotelId { get; set; }
        
        #endregion
    }
}
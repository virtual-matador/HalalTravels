using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel contract document search model
    /// </summary>
    public partial class HotelContractDocumentSearchModel : BaseSearchModel
    {
        #region Properties

        public int HotelId { get; set; }
        
        #endregion
    }
}
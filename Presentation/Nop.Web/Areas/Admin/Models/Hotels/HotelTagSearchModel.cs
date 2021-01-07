using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a product tag search model
    /// </summary>
    public partial class HotelTagSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Admin.Hotels.HotelTags.Fields.SearchTagName")]
        public string SearchTagName { get; set; }
    }
}
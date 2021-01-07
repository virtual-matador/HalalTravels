using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a chain search model
    /// </summary>
    public partial class ChainSearchModel : BaseSearchModel
    {
        #region Ctor

        public ChainSearchModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Chain.List.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }

        #endregion
    }
}

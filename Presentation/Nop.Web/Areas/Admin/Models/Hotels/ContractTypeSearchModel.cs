using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class ContractTypeSearchModel : BaseSearchModel
    {
        #region Ctor

        public ContractTypeSearchModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.ContractType.List.SearchContractTypeName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }

        #endregion
    }
}
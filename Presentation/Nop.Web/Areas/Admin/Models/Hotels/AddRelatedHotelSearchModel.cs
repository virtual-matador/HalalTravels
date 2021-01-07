using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class AddRelatedHotelSearchModel : BaseSearchModel
    {
        #region Ctor

        public AddRelatedHotelSearchModel()
        {
            AvailableChains = new List<SelectListItem>();
            AvailableHotelTypes = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelName")]
        public string SearchHotelName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchChainId")]
        public int SearchChainId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelTypeId")]
        public int SearchHotelTypeId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailableChains { get; set; }

        public IList<SelectListItem> AvailableHotelTypes { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public bool IsLoggedInAsVendor { get; set; }

        #endregion

    }
}

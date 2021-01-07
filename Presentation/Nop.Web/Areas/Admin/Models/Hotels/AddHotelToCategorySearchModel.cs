using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class AddHotelToCategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public AddHotelToCategorySearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableHotelTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelName")]
        public string SearchHotelName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchStore")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchVendor")]
        public int SearchVendorId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelType")]
        public int SearchHotelTypeId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableHotelTypes { get; set; }

        #endregion
    }
}

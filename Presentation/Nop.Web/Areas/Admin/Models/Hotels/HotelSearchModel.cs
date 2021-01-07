using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel search model
    /// </summary>
    public partial class HotelSearchModel : BaseSearchModel
    {
        #region Ctor

        public HotelSearchModel()
        {
            AvailablePublishedOptions = new List<SelectListItem>();
            AvailableChainOptions = new List<SelectListItem>();
            AvailableHotelTypeOptions = new List<SelectListItem>();
            AvailableCountryOptions = new List<SelectListItem>();
            AvailableProvinceOptions = new List<SelectListItem>();
            AvailableCountyOptions = new List<SelectListItem>();
            AvailableCityOptions = new List<SelectListItem>();
            AvailableCategoryOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelName")]
        public string SearchHotelName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelAddress")]
        public string SearchHotelAddress { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchPublished")]
        public bool SearchPublished { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCountryId")]
        public int SearchCountryId { get; set; }

        public IList<SelectListItem> AvailableCountryOptions { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchProvinceId")]
        public int SearchProvinceId { get; set; }

        public IList<SelectListItem> AvailableProvinceOptions { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCountyId")]
        public int SearchCountyId { get; set; }

        public IList<SelectListItem> AvailableCountyOptions { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCityId")]
        public int SearchCityId { get; set; }

        public IList<SelectListItem> AvailableCityOptions { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchHotelTypeId")]
        public int SearchHotelTypeId { get; set; }

        public IList<SelectListItem> AvailableHotelTypeOptions { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchChainId")]
        public int SearchChainId { get; set; }

        public IList<SelectListItem> AvailableChainOptions { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.List.SearchCategoryId")]
        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailableCategoryOptions { get; set; }

        #endregion
    }
}

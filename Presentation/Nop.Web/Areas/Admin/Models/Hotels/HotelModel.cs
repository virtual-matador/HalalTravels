using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel model
    /// </summary>
    public partial class HotelModel : BaseNopEntityModel, ILocalizedModel<HotelLocalizedModel>, IStoreMappingSupportedModel, IAclSupportedModel
    {
        #region Ctor

        public HotelModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<HotelLocalizedModel>();

            AvailableHotelTypes = new List<SelectListItem>();
            AvailableChains = new List<SelectListItem>();
            AvailableCountries = new List<SelectListItem>();
            AvailableProvinces = new List<SelectListItem>();
            AvailableCounties = new List<SelectListItem>();
            AvailableCities = new List<SelectListItem>();
            HotelPictureModels = new List<HotelPictureModel>();
            AddPictureModel = new HotelPictureModel();
            HotelPictureSearchModel = new HotelPictureSearchModel();
            RelatedHotelSearchModel = new RelatedHotelSearchModel();
            
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();

            SelectedCategoryIds = new List<int>();
            AvailableCategories = new List<SelectListItem>();

            SelectedCountryLimitationIds = new List<int>();

            AvailableTaxCategories = new List<SelectListItem>();
            AvailablePricingModels = new List<SelectListItem>();
            AvailableContractTypes = new List<SelectListItem>();
            AvailableCurrencies = new List<SelectListItem>();
            HotelContactSearchModel = new HotelContactSearchModel();
            HotelContractDocumentSearchModel = new HotelContractDocumentSearchModel();
            AddHotelContractDocumentModel = new HotelContractDocumentModel();
            HotelContractDocumentModels = new List<HotelContractDocumentModel>();

            SelectedCityIds = new List<int>();
        }

        #endregion

        #region Properties
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Name")]
        public string Name { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.HotelTypeId")]
        public int HotelTypeId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ChainId")]
        public int? ChainId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AddressLine1")]
        public string AddressLine1 { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AddressLine2")]
        public string AddressLine2 { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Latitude")]
        public float? Latitude { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Longitude")]
        public float? Longitude { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.PostCode")]
        public string PostCode { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Tel1")]
        public string Tel1 { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Tel2")]
        public string Tel2 { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AllowCustomerReviews")]
        public bool AllowCustomerReviews { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AvailableStartDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableStartDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AvailableEndDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableEndDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.LongDescription")]
        public string LongDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CountyId")]
        public int? CountyId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CountyName")]
        public string CountyName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ProvinceId")]
        public int ProvinceId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ProvinceName")]
        public string ProvinceName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CountryId")]
        public int CountryId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CountryName")]
        public string CountryName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.HotelTypeName")]
        public string HotelTypeName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ChainName")]
        public string ChainName { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.SeName")]
        public string SeName { get; set; }

        //picture thumbnail
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.PictureThumbnailUrl")]
        public string PictureThumbnailUrl { get; set; }

        public IList<HotelLocalizedModel> Locales { get; set; }

        public IList<SelectListItem> AvailableHotelTypes { get; set; }

        public IList<SelectListItem> AvailableChains { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public IList<SelectListItem> AvailableProvinces { get; set; }

        public IList<SelectListItem> AvailableCounties { get; set; }
        
        public IList<SelectListItem> AvailableCities { get; set; }
        
        //ACL (customer roles)
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AclCustomerRoles")]
        
        public IList<int> SelectedCustomerRoleIds { get; set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.LimitedToStores")] 
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        //pictures
        public HotelPictureModel AddPictureModel { get; set; }

        public IList<HotelPictureModel> HotelPictureModels { get; set; }

        public HotelPictureSearchModel HotelPictureSearchModel { get; set; }

        public RelatedHotelSearchModel RelatedHotelSearchModel { get; set; }

        //categories
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Categories")]
        public IList<int> SelectedCategoryIds { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.HotelTags")]
        public string HotelTags { get; set; }

        public string InitialHotelTags { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CountryLimitations")]
        public IList<int> SelectedCountryLimitationIds { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.TaxCategory")]
        public int? TaxCategoryId { get; set; }
        
        public IList<SelectListItem> AvailableTaxCategories { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.DefaultCurrencyId")]
        public int? DefaultCurrencyId { get; set; }
        
        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.BasepriceEnabled")]
        public bool BasepriceEnabled { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.PricingModelId")]
        public int PricingModelId { get; set; }
        
        public IList<SelectListItem> AvailablePricingModels { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ContractTypeId")]
        public int? ContractTypeId { get; set; }
        
        public IList<SelectListItem> AvailableContractTypes { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Contractor")]
        public string Contractor { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ContractStartDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? ContractStartDateTimeUtc { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ContractEndDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? ContractEndDateTimeUtc { get; set; }public string AccountCode { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AccountName")]
        public string AccountName { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AccountCurrencyId")]
        public int? AccountCurrencyId { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.TaxNumber")]
        public string TaxNumber { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CheckInTime")]
        public string CheckInTime { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CheckOutTime")]
        public string CheckOutTime { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CheckInAndOutPolicy")]
        public string CheckInAndOutPolicy { get; set; }
        
        public HotelContactSearchModel HotelContactSearchModel { get; set; }
        
        public HotelContractDocumentSearchModel HotelContractDocumentSearchModel { get; set; }
        
        public HotelContractDocumentModel AddHotelContractDocumentModel { get; set; }
        
        public IList<HotelContractDocumentModel> HotelContractDocumentModels { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Cities")]
        public IList<int> SelectedCityIds { get; set; }

        #endregion
    }

    public partial class HotelLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.LongDescription")]
        public string LongDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.AdminComment")]
        public string AdminComment { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.SeName")]
        public string SeName { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.Fields.CheckInAndOutPolicy")]
        public string CheckInAndOutPolicy { get; set; }
    }
}

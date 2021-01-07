using System;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Hotels
{
    public partial class Hotel : BaseEntity, ILocalizedEntity, ISlugSupported, IAclSupported
    {
        #region Properies

        public string Name { get; set; }
        
        public string MetaKeywords { get; set; }
        
        public string MetaTitle { get; set; }
        
        public string MetaDescription { get; set; }

        public int HotelTypeId { get; set; }

        public int? ChainId { get; set; }
        
        public int? CountyId { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public float? Latitude { get; set; }
        
        public float? Longitude { get; set; }
        
        public string PostCode { get; set; }
        
        public string Tel1 { get; set; }
        
        public string Tel2 { get; set; }
        
        public bool Published { get; set; }

        public bool AllowCustomerReviews { get; set; }

        public DateTime? AvailableStartDateTimeUtc { get; set; }

        public DateTime? AvailableEndDateTimeUtc { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string AdminComment { get; set; }

        public int DisplayOrder { get; set; }

        public bool SubjectToAcl { get; set; }
        
        public bool LimitedToCountries { get; set; }
        
        public int? DefaultCurrencyId { get; set; }
        
        public bool DisableWishlistButton { get; set; }
        
        public bool BasepriceEnabled { get; set; }
        
        public bool IsTaxExempt { get; set; }
        
        public int? TaxCategoryId { get; set; }
        
        public int PricingModelId { get; set; }
        
        public int? ContractTypeId { get; set; }
        
        public string Contractor { get; set; }
        
        public DateTime? ContractStartDateTimeUtc { get; set; }
        
        public DateTime? ContractEndDateTimeUtc { get; set; }
        
        public string AccountCode { get; set; }
        
        public string AccountName { get; set; }
        
        public int? AccountCurrencyId { get; set; }
        
        public string TaxNumber { get; set; }
        
        public string CheckInTime { get; set; }
        
        public string CheckOutTime { get; set; }
        
        public string CheckInAndOutPolicy { get; set; }
        
        #endregion
    }
}

using AutoMapper;
using Nop.Core.Domain.Hotels;
using Nop.Core.Infrastructure.Mapper;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for halal travels admin area models
    /// </summary>
    public class HalalTravelsAdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public HalalTravelsAdminMapperConfiguration()
        {
            CreateHotelMaps();
        }

        #endregion

        #region Utilities

        protected virtual void CreateHotelMaps()
        {
            CreateMap<Chain, ChainModel>()
                .ForMember(model => model.PageSize, options => options.Ignore())
                .ForMember(model => model.Locales, options => options.Ignore());

            CreateMap<ChainModel, Chain>();

            CreateMap<Hotel, HotelModel>()
                .ForMember(model => model.PageSize, options => options.Ignore())
                .ForMember(model => model.AllowCustomersToSelectPageSize, options => options.Ignore())
                .ForMember(model => model.AvailableHotelTypes, options => options.Ignore())
                .ForMember(model => model.AvailableChains, options => options.Ignore())
                .ForMember(model => model.AvailableCountries, options => options.Ignore())
                .ForMember(model => model.AvailableProvinces, options => options.Ignore())
                .ForMember(model => model.AvailableCities, options => options.Ignore())
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.AvailableTaxCategories, options => options.Ignore())
                .ForMember(model => model.Locales, options => options.Ignore())
                .ForMember(model => model.ChainName, options => options.Ignore())
                .ForMember(model => model.HotelTypeName, options => options.Ignore())
                .ForMember(model => model.CountryName, options => options.Ignore())
                .ForMember(model => model.ProvinceName, options => options.Ignore())
                .ForMember(model => model.CountyName, options => options.Ignore())
                .ForMember(model => model.CountryId, options => options.Ignore())
                .ForMember(model => model.CountyId, options => options.Ignore())
                .ForMember(model => model.ProvinceId, options => options.Ignore())
                .ForMember(model => model.SelectedCityIds, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.Ignore())
                .ForMember(model => model.AddPictureModel, options => options.Ignore())
                .ForMember(model => model.HotelPictureModels, options => options.Ignore())
                .ForMember(model => model.HotelPictureSearchModel, options => options.Ignore())
                .ForMember(model => model.RelatedHotelSearchModel, options => options.Ignore())
                .ForMember(model => model.HotelTags, options => options.Ignore())
                .ForMember(model => model.InitialHotelTags, options => options.Ignore())
                .ForMember(model => model.AvailablePricingModels, options => options.Ignore())
                .ForMember(model => model.AvailableContractTypes, options => options.Ignore())
                .ForMember(model => model.AvailableCurrencies, options => options.Ignore())
                .ForMember(model => model.HotelContractDocumentModels, options => options.Ignore())
                .ForMember(model => model.AddHotelContractDocumentModel, options => options.Ignore())
                .ForMember(model => model.HotelContractDocumentSearchModel, options => options.Ignore());

            CreateMap<HotelModel, Hotel>();

            CreateMap<HotelPicture, HotelPictureModel>()
                .ForMember(model => model.OverrideAltAttribute, options => options.Ignore())
                .ForMember(model => model.OverrideTitleAttribute, options => options.Ignore())
                .ForMember(model => model.PictureUrl, options => options.Ignore());

            CreateMap<HotelPictureModel, HotelPicture>();

            CreateMap<HotelType, HotelTypeModel>()
                .ForMember(model => model.PageSize, options => options.Ignore());

            CreateMap<HotelTypeModel, HotelType>();

            CreateMap<RelatedHotel, RelatedHotelModel>()
                .ForMember(model => model.RelatedHotelName, options => options.Ignore());

            CreateMap<RelatedHotelModel, RelatedHotel>();

            CreateMap<HotelCategory, CategoryHotelModel>()
                .ForMember(model => model.HotelName, options => options.Ignore());

            CreateMap<CategoryHotelModel, HotelCategory>()
                .ForMember(entity => entity.CategoryId, options => options.Ignore())
                .ForMember(entity => entity.HotelId, options => options.Ignore());

            CreateMap<Category, CategoryModel>()
                .ForMember(model => model.CategoryHotelSearchModel, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.Ignore());

            CreateMap<CategoryModel, Category>();

            CreateMap<HotelTag, HotelTagModel>()
                .ForMember(model => model.HotelCount, options => options.Ignore());

            CreateMap<HotelTagModel, HotelTag>();
            
            CreateMap<HotelLimitedToCountry, HotelTagModel>()
                .ForMember(model => model.HotelCount, options => options.Ignore());

            CreateMap<HotelTagModel, HotelLimitedToCountry>();
            
            CreateMap<PricingModel, PricingModelModel>()
                .ForMember(model => model.PageSize, options => options.Ignore());

            CreateMap<PricingModelModel, PricingModel>();
            
            CreateMap<ContractType, ContractTypeModel>()
                .ForMember(model => model.PageSize, options => options.Ignore());

            CreateMap<ContractTypeModel, ContractType>();
            
            CreateMap<HotelContact, HotelContactModel>()
                .ForMember(model => model.PageSize, options => options.Ignore())
                .ForMember(model => model.DepartmentName, options => options.Ignore())
                .ForMember(model => model.AvailableDepartments, options => options.Ignore());

            CreateMap<HotelContactModel, HotelContact>();
            
            CreateMap<HotelContractDocument, HotelContractDocumentModel>()
                .ForMember(model => model.OverrideAltAttribute, options => options.Ignore())
                .ForMember(model => model.OverrideTitleAttribute, options => options.Ignore())
                .ForMember(model => model.DocumentUrl, options => options.Ignore());

            CreateMap<HotelContractDocumentModel, HotelContractDocument>();
            
            CreateMap<HotelCityMapping, HotelCityMappingModel>()
                .ForMember(model => model.CityName, options => options.Ignore());

            CreateMap<HotelCityMappingModel, HotelCityMapping>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 1;

        #endregion
    }
}

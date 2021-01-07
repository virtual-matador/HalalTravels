using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Tax;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Hotels;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Zones;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using ICountryService = Nop.Services.Zones.ICountryService;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the hotel model factory implementation
    /// </summary>
    public partial class HotelModelFactory : IHotelModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IHotelService _hotelService;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly TaxSettings _taxSettings;

        protected readonly IChainService _chainService;
        protected readonly IHotelTypeService _hotelTypeService;
        protected readonly ICountryService _countryService;
        protected readonly IProvinceService _provinceService;
        protected readonly ICountyService _countyService;
        protected readonly ICityService _cityService;
        protected readonly IHotelTagService _hotelTagService;
        protected readonly ICategoryService _categoryService;
        protected readonly IDepartmentService _departmentService;
        protected readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public HotelModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            IHotelService hotelService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IPictureService pictureService,
            TaxSettings taxSettings,
            IChainService chainService,
            IHotelTypeService hotelTypeService,
            ICountryService countryService,
            IProvinceService provinceService,
            ICountyService countyService,
            ICityService cityService,
            IHotelTagService hotelTagService,
            ICategoryService categoryService,
            IUrlRecordService urlRecordService,
            IDepartmentService departmentService,
            IWorkContext workContext)
        {
            _currencySettings = currencySettings;
            _aclSupportedModelFactory = aclSupportedModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _hotelService = hotelService;
            _currencyService = currencyService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _taxSettings = taxSettings;
            _pictureService = pictureService;

            _chainService = chainService;
            _hotelTypeService = hotelTypeService;
            _countryService = countryService;
            _provinceService = provinceService;
            _countyService = countyService;
            _cityService = cityService;
            _hotelTagService = hotelTagService;
            _categoryService = categoryService;
            _departmentService = departmentService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
        }

        #endregion

        /// <summary>
        /// Prepare hotel search model
        /// </summary>
        /// <param name="searchModel">Hotel search model</param>
        /// <returns>Hotel search model</returns>
        public virtual HotelSearchModel PrepareHotelSearchModel(HotelSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            _baseAdminModelFactory.PrepareChains(searchModel.AvailableChainOptions);
            _baseAdminModelFactory.PrepareHotelTypes(searchModel.AvailableHotelTypeOptions);
            _baseAdminModelFactory.PrepareHotelCountries(searchModel.AvailableCountryOptions);
            _baseAdminModelFactory.PrepareHotelCategories(searchModel.AvailableCategoryOptions);

            //prepare "published" filter (0 - all; 1 - published only; 2 - unpublished only)
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = _localizationService.GetResource("Admin.Hotels.Hotel.List.SearchPublished.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = _localizationService.GetResource("Admin.Hotels.Hotel.List.SearchPublished.PublishedOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = _localizationService.GetResource("Admin.Hotels.Hotel.List.SearchPublished.UnpublishedOnly")
            });

            searchModel.SearchPublished = true;

            //prepare grid
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged hotel list model
        /// </summary>
        /// <param name="searchModel">Hotel search model</param>
        /// <returns>Hotel list model</returns>
        public virtual HotelListModel PrepareHotelListModel(HotelSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var categoryIds = new List<int> {searchModel.SearchCategoryId};

            var hotels = _hotelService.SearchHotels(showHidden: true,
                categoryIds: categoryIds,
                hotelTypeIds: new List<int> {searchModel.SearchHotelTypeId},
                chainIds: new List<int> {searchModel.SearchChainId},
                countryId: searchModel.SearchCountryId,
                provinceId: searchModel.SearchProvinceId,
                countyId: searchModel.SearchCountyId,
                cityId: searchModel.SearchCityId,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublished);
            /*overridePublished: searchModel.SearchPublishedId == 0
                ? null
                : (bool?) (searchModel.SearchPublishedId == 1));*/

            //prepare grid model
            var model = new HotelListModel().PrepareToGrid(searchModel, hotels, () =>
            {
                return hotels.Select(hotel =>
                {
                    var hotelModel = hotel.ToModel<HotelModel>();

                    hotelModel.SeName = _urlRecordService.GetSeName(hotel, 0, true, false);

                    if (hotel.CountyId != null)
                    {
                        var county = _countyService.GetCountyById(hotel.CountyId.Value);
                        if (county != null)
                        {
                            hotelModel.CountyId = county.Id;
                            hotelModel.CountyName = county.Name;

                            var province = _provinceService.GetProvinceById(county.ProvinceId);

                            if (province != null)
                            {
                                hotelModel.ProvinceId = province.Id;
                                hotelModel.ProvinceName = province.Name;

                                var country = _countryService.GetCountryById(province.CountryId);
                                hotelModel.CountryId = country?.Id ?? 0;
                                hotelModel.CountryName = country?.Name;
                            }
                        }
                    }

                    if (hotel.ChainId != null)
                    {
                        var chain = _chainService.GetChainById(hotel.ChainId ?? 0);
                        hotelModel.ChainName = chain?.Name;
                    }

                    if (hotel.HotelTypeId != 0)
                    {
                        var hotelType = _hotelTypeService.GetHotelTypeById(hotel.HotelTypeId);
                        hotelModel.HotelTypeName = hotelType?.Name;
                    }


                    var defaultHotelPicture = _hotelService.GetPicturesByHotelId(hotel.Id, 1).FirstOrDefault();
                    hotelModel.PictureThumbnailUrl = _pictureService.GetPictureUrl(ref defaultHotelPicture, 75);

                    return hotelModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare hotel model
        /// </summary>
        /// <param name="model">Hotel model</param>
        /// <param name="hotel">Hotel</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Hotel model</returns>
        public virtual HotelModel PrepareHotelModel(HotelModel model, Hotel hotel, bool excludeProperties = false)
        {
            Action<HotelLocalizedModel, int> localizedModelConfiguration = null;

            if (hotel != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = hotel.ToModel<HotelModel>();
                    model.SeName = _urlRecordService.GetSeName(hotel, 0, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(hotel, entity => entity.Name, languageId, false, false);
                    locale.ShortDescription = _localizationService.GetLocalized(hotel, entity => entity.ShortDescription, languageId, false, false);
                    locale.LongDescription = _localizationService.GetLocalized(hotel, entity => entity.LongDescription, languageId, false, false);
                    locale.AdminComment = _localizationService.GetLocalized(hotel, entity => entity.AdminComment, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(hotel, languageId, false, false);
                };

                PrepareHotelPictureSearchModel(model.HotelPictureSearchModel, hotel);
                PrepareHotelPictureListModel(model.HotelPictureSearchModel, hotel);

                PrepareRelatedHotelSearchModel(model.RelatedHotelSearchModel, hotel);
                PrepareRelatedHotelListModel(model.RelatedHotelSearchModel, hotel);

                PrepareHotelContactSearchModel(model.HotelContactSearchModel, hotel);

                PrepareContractDocumentSearchModel(model.HotelContractDocumentSearchModel, hotel);
                PrepareContractDocumentListModel(model.HotelContractDocumentSearchModel, hotel);

                model.HotelTags = string.Join(", ", _hotelTagService.GetAllHotelTagsByHotelId(hotel.Id).Select(tag => tag.Name));

                if (!excludeProperties)
                {
                    model.SelectedCategoryIds = _categoryService.GetHotelCategoriesByHotelId(hotel.Id, showHidden: true)
                        .Select(hotelCategory => hotelCategory.CategoryId).ToList();

                    model.SelectedCountryLimitationIds = _hotelService.GetHotelLimitedToCountriesByHotelId(hotel.Id, true)
                        .Select(limitation => limitation.CountryId).ToList();

                    model.SelectedCityIds = _hotelService.GetHotelCityMappingsByHotelId(hotel.Id, true)
                        .Select(city => city.CityId).ToList();
                }
            }

            //set default values for the new model
            if (hotel == null)
            {
                model.Published = true;
                model.AllowCustomersToSelectPageSize = true;
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            var countryId = 0;
            var provinceId = 0;
            var countyId = 0;

            if (model.CountyId != null)
            {
                var county = _countyService.GetCountyById(countyId);
                model.CountyName = county?.Name;
                provinceId = county?.ProvinceId ?? 0;

                if (provinceId != 0)
                {
                    var province = _provinceService.GetProvinceById(provinceId);
                    model.ProvinceName = province?.Name;
                    countryId = province?.CountryId ?? 0;
                    model.CountryId = countryId;

                    if (countryId != 0)
                    {
                        var country = _countryService.GetCountryById(countryId);
                        model.CountryName = country?.Name;
                    }
                }
            }

            _baseAdminModelFactory.PrepareChains(model.AvailableChains,
                defaultItemText: _localizationService.GetResource("Admin.Hotels.Hotel.Fields.Chain.None"));
            _baseAdminModelFactory.PrepareHotelTypes(model.AvailableHotelTypes, false);
            _baseAdminModelFactory.PrepareHotelCountries(model.AvailableCountries,
                defaultItemText: _localizationService.GetResource("Admin.Hotels.Hotel.Fields.Country.None"));
            _baseAdminModelFactory.PrepareHotelProvinces(model.AvailableProvinces, countryId, false);
            _baseAdminModelFactory.PrepareHotelCounties(model.AvailableCounties, provinceId, false);
            _baseAdminModelFactory.PrepareHotelCities(model.AvailableCities, provinceId, false);
            _baseAdminModelFactory.PrepareTaxCategories(model.AvailableTaxCategories,
                defaultItemText: _localizationService.GetResource("Admin.Hotels.Hotel.Fields.TaxCategory.None"));
            _baseAdminModelFactory.PreparePricingModels(model.AvailablePricingModels, false);
            _baseAdminModelFactory.PrepareContractTypes(model.AvailableContractTypes,
                defaultItemText: _localizationService.GetResource("Admin.Hotels.Hotel.Fields.ContractType.None"));
            _baseAdminModelFactory.PrepareCurrencies(model.AvailableCurrencies,
                defaultItemText: _localizationService.GetResource("Admin.Hotels.Hotel.Fields.Currency.None"));

            //prepare model categories
            _baseAdminModelFactory.PrepareHotelCategories(model.AvailableCategories, false);
            foreach (var categoryItem in model.AvailableCategories)
            {
                categoryItem.Selected = int.TryParse(categoryItem.Value, out var categoryId)
                                        && model.SelectedCategoryIds.Contains(categoryId);
            }

            foreach (var countryItem in model.AvailableCountries)
            {
                countryItem.Selected = int.TryParse(countryItem.Value, out var limitedCountryId)
                                       && model.SelectedCountryLimitationIds.Contains(limitedCountryId);
            }

            foreach (var cityItem in model.AvailableCities)
            {
                cityItem.Selected = int.TryParse(cityItem.Value, out var cityId)
                                    && model.SelectedCityIds.Contains(cityId);
            }

            //prepare model customer roles
            _aclSupportedModelFactory.PrepareModelCustomerRoles(model, hotel, excludeProperties);

            var hotelTags = _hotelTagService.GetAllHotelTags();
            var hotelTagsSb = new StringBuilder();
            hotelTagsSb.Append("var initialHotelTags = [");
            for (var i = 0; i < hotelTags.Count; i++)
            {
                var tag = hotelTags[i];
                hotelTagsSb.Append("'");
                hotelTagsSb.Append(JavaScriptEncoder.Default.Encode(tag.Name));
                hotelTagsSb.Append("'");
                if (i != hotelTags.Count - 1)
                    hotelTagsSb.Append(",");
            }

            hotelTagsSb.Append("]");

            model.InitialHotelTags = hotelTagsSb.ToString();

            return model;
        }

        /// <summary>
        /// Prepare hotel picture search model
        /// </summary>
        /// <param name="searchModel">Hotel picture search model</param>
        /// <param name="hotel">Hotel</param>
        /// <returns>Hotel picture search model</returns>
        protected virtual HotelPictureSearchModel PrepareHotelPictureSearchModel(HotelPictureSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            searchModel.HotelId = hotel.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged hotel picture list model
        /// </summary>
        /// <param name="searchModel">Hotel picture search model</param>
        /// <param name="hotel">Hotel</param>
        /// <returns>Hotel picture list model</returns>
        public virtual HotelPictureListModel PrepareHotelPictureListModel(HotelPictureSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //get hotel pictures
            var hotelPictures = _hotelService.GetHotelPicturesByHotelId(hotel.Id).ToPagedList(searchModel);

            //prepare grid model
            var model = new HotelPictureListModel().PrepareToGrid(searchModel, hotelPictures, () =>
            {
                return hotelPictures.Select(hotelPicture =>
                {
                    //fill in model values from the entity
                    var hotelPictureModel = hotelPicture.ToModel<HotelPictureModel>();

                    //fill in additional values (not existing in the entity)
                    var picture = _pictureService.GetPictureById(hotelPicture.PictureId)
                                  ?? throw new Exception("Picture cannot be loaded");

                    hotelPictureModel.PictureUrl = _pictureService.GetPictureUrl(ref picture);
                    hotelPictureModel.OverrideAltAttribute = picture.AltAttribute;
                    hotelPictureModel.OverrideTitleAttribute = picture.TitleAttribute;

                    return hotelPictureModel;
                });
            });

            return model;
        }

        protected virtual RelatedHotelSearchModel PrepareRelatedHotelSearchModel(RelatedHotelSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            searchModel.HotelId = hotel.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged related hotel list model
        /// </summary>
        /// <param name="searchModel">Related hotel search model</param>
        /// <param name="hotel">Hotel</param>
        /// <returns>Related hotel list model</returns>
        public virtual RelatedHotelListModel PrepareRelatedHotelListModel(RelatedHotelSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //get related hotels
            var relatedHotels = _hotelService.GetRelatedHotelsByHotelId(hotelId: hotel.Id, showHidden: true).ToPagedList(searchModel);

            //prepare grid model
            var model = new RelatedHotelListModel().PrepareToGrid(searchModel, relatedHotels, () =>
            {
                return relatedHotels.Select(relatedHotel =>
                {
                    //fill in model values from the entity
                    var relatedHotelModel = relatedHotel.ToModel<RelatedHotelModel>();

                    //fill in additional values (not existing in the entity)
                    relatedHotelModel.RelatedHotelName = _hotelService.GetHotelById(relatedHotel.RelatedHotelId)?.Name;

                    return relatedHotelModel;
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare related hotel search model to add to the hotel
        /// </summary>
        /// <param name="searchModel">Related hotel search model to add to the hotel</param>
        /// <returns>Related hotel search model to add to the hotel</returns>
        public virtual AddRelatedHotelSearchModel PrepareAddRelatedHotelSearchModel(AddRelatedHotelSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            _baseAdminModelFactory.PrepareChains(searchModel.AvailableChains);

            _baseAdminModelFactory.PrepareHotelTypes(searchModel.AvailableHotelTypes);

            _baseAdminModelFactory.PrepareHotelCategories(searchModel.AvailableCategories);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged related hotel list model to add to the hotel
        /// </summary>
        /// <param name="searchModel">Related hotel search model to add to the hotel</param>
        /// <returns>Related hotel list model to add to the hotel</returns>
        public virtual AddRelatedHotelListModel PrepareAddRelatedHotelListModel(AddRelatedHotelSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //a vendor should have access only to his hotels
            //if (_workContext.CurrentVendor != null)
            //    searchModel.SearchVendorId = _workContext.CurrentVendor.Id;

            //get hotels
            var hotels = _hotelService.SearchHotels(showHidden: true,
                categoryIds: new List<int> {searchModel.SearchCategoryId},
                chainIds: new List<int> {searchModel.SearchChainId},
                hotelTypeIds: new List<int> {searchModel.SearchHotelTypeId},

                keywords: searchModel.SearchHotelName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model
            var model = new AddRelatedHotelListModel().PrepareToGrid(searchModel, hotels, () =>
            {
                return hotels.Select(hotel =>
                {
                    var hotelModel = hotel.ToModel<HotelModel>();
                    hotelModel.SeName = _urlRecordService.GetSeName(hotel, 0, true, false);

                    return hotelModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare hotel tag search model
        /// </summary>
        /// <param name="searchModel">Hotel tag search model</param>
        /// <returns>Hotel tag search model</returns>
        public virtual HotelTagSearchModel PrepareHotelTagSearchModel(HotelTagSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged hotel tag list model
        /// </summary>
        /// <param name="searchModel">Hotel tag search model</param>
        /// <returns>Hotel tag list model</returns>
        public virtual HotelTagListModel PrepareHotelTagListModel(HotelTagSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get hotel tags
            var hotelTags = _hotelTagService.GetAllHotelTags(tagName: searchModel.SearchTagName)
                .OrderByDescending(tag => _hotelTagService.GetHotelCount(tag.Id, storeId: 0, showHidden: true)).ToList()
                .ToPagedList(searchModel);

            //prepare list model
            var model = new HotelTagListModel().PrepareToGrid(searchModel, hotelTags, () =>
            {
                return hotelTags.Select(tag =>
                {
                    //fill in model values from the entity
                    var hotelTagModel = tag.ToModel<HotelTagModel>();

                    //fill in additional values (not existing in the entity)
                    hotelTagModel.HotelCount = _hotelTagService.GetHotelCount(tag.Id, storeId: 0, showHidden: true);

                    return hotelTagModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare hotel tag model
        /// </summary>
        /// <param name="model">Hotel tag model</param>
        /// <param name="hotelTag">Hotel tag</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Hotel tag model</returns>
        public virtual HotelTagModel PrepareHotelTagModel(HotelTagModel model, HotelTag hotelTag, bool excludeProperties = false)
        {
            Action<HotelTagLocalizedModel, int> localizedModelConfiguration = null;

            if (hotelTag != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = hotelTag.ToModel<HotelTagModel>();
                }

                model.HotelCount = _hotelTagService.GetHotelCount(hotelTag.Id, storeId: 0, showHidden: true);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) => { locale.Name = _localizationService.GetLocalized(hotelTag, entity => entity.Name, languageId, false, false); };
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        public virtual HotelContactListModel PrepareHotelContactListModel(HotelContactSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            var hotelContacts = _hotelService.GetHotelContactsByHotelId(hotel.Id).ToPagedList(searchModel);

            var model = new HotelContactListModel().PrepareToGrid(searchModel, hotelContacts, () =>
            {
                return hotelContacts.Select(contact =>
                {
                    //fill in model values from the entity
                    var hotelContactModel = contact.ToModel<HotelContactModel>();

                    var department = _departmentService.GetDepartmentById(contact.DepartmentId);
                    hotelContactModel.DepartmentName = department?.Name;

                    return hotelContactModel;
                });
            });

            return model;
        }

        public virtual HotelContactModel PrepareHotelContactModel(HotelContactModel model, Hotel hotel, HotelContact hotelContact, bool excludeProperties = false)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            Action<HotelContactLocalizedModel, int> localizedModelConfiguration = null;

            if (hotelContact != null)
            {
                //fill in model values from the entity
                model ??= hotelContact.ToModel<HotelContactModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(hotelContact, entity => entity.Name, languageId, false, false);
                    locale.Surename = _localizationService.GetLocalized(hotelContact, entity => entity.Surename, languageId, false, false);
                    locale.Position = _localizationService.GetLocalized(hotelContact, entity => entity.Position, languageId, false, false);
                };
            }

            model.HotelId = hotel.Id;

            _baseAdminModelFactory.PrepareDepartments(model.AvailableDepartments);

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        public virtual HotelContactSearchModel PrepareHotelContactSearchModel(HotelContactSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            searchModel.HotelId = hotel.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual HotelContractDocumentSearchModel PrepareContractDocumentSearchModel(HotelContractDocumentSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            searchModel.HotelId = hotel.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual HotelContractDocumentListModel PrepareContractDocumentListModel(HotelContractDocumentSearchModel searchModel, Hotel hotel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //get hotel contract document
            var hotelContractDocuments = _hotelService.GetHotelContractDocumentsByHotelId(hotel.Id).ToPagedList(searchModel);

            //prepare grid model
            var model = new HotelContractDocumentListModel().PrepareToGrid(searchModel, hotelContractDocuments, () =>
            {
                return hotelContractDocuments.Select(hotelContractDocument =>
                {
                    //fill in model values from the entity
                    var hotelContractDocumentModel = hotelContractDocument.ToModel<HotelContractDocumentModel>();

                    //fill in additional values (not existing in the entity)
                    var picture = _pictureService.GetPictureById(hotelContractDocument.DocumentId)
                                  ?? throw new Exception("Document cannot be loaded");

                    hotelContractDocumentModel.DocumentUrl = _pictureService.GetPictureUrl(ref picture);
                    hotelContractDocumentModel.OverrideAltAttribute = picture.AltAttribute;
                    hotelContractDocumentModel.OverrideTitleAttribute = picture.TitleAttribute;

                    return hotelContractDocumentModel;
                });
            });

            return model;
        }

        public virtual HotelSelectorModel PrepareHotelSelectorModel()
        {
            var model = new HotelSelectorModel
            {
                CurrentHotel = _workContext.DefaultHotel?.ToModel<HotelModel>(),
                AvailableHotels = _hotelService.GetAllHotels(true)
                    .Select(hotel => hotel.ToModel<HotelModel>()).ToList()
            };

            var defaultItemText = _localizationService.GetResource(model.CurrentHotel == null ? "Admin.Common.SelectDefaultHotel" : "Admin.Common.NoDefaultHotel");
            model.AvailableHotels.Insert(0, new HotelModel {Id = 0, Name = defaultItemText});

            return model;
        }
    }
}
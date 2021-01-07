using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    /// Represents the hotelType model factory implementation
    /// </summary>
    public partial class HotelTypeModelFactory : IHotelTypeModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IHotelTypeService _hotelTypeService;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly TaxSettings _taxSettings;

        protected readonly IChainService _chainService;
        protected readonly ICountryService _countryService;
        protected readonly IProvinceService _provinceService;
        protected readonly ICityService _cityService;

        #endregion

        #region Ctor

        public HotelTypeModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            IHotelTypeService hotelTypeService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IPictureService pictureService,
            TaxSettings taxSettings, 
            IChainService chainService, 
            ICountryService countryService, 
            IProvinceService provinceService, 
            ICityService cityService,
            IUrlRecordService urlRecordService)
        {
            _currencySettings = currencySettings;
            _aclSupportedModelFactory = aclSupportedModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _hotelTypeService = hotelTypeService;
            _currencyService = currencyService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _taxSettings = taxSettings;
            _pictureService = pictureService;

            _chainService = chainService;
            _countryService = countryService;
            _provinceService = provinceService;
            _cityService = cityService;
            _urlRecordService = urlRecordService;
        }

        #endregion

        /// <summary>
        /// Prepare hotelType search model
        /// </summary>
        /// <param name="searchModel">HotelType search model</param>
        /// <returns>HotelType search model</returns>
        public virtual HotelTypeSearchModel PrepareHotelTypeSearchModel(HotelTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            
            //prepare "published" filter (0 - all; 1 - published only; 2 - unpublished only)
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.PublishedOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = _localizationService.GetResource("Admin.Catalog.Products.List.SearchPublished.UnpublishedOnly")
            });

            //prepare grid
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged hotelType list model
        /// </summary>
        /// <param name="searchModel">HotelType search model</param>
        /// <returns>HotelType list model</returns>
        public virtual HotelTypeListModel PrepareHotelTypeListModel(HotelTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var hotelTypes = _hotelTypeService.SearchHotelTypes(showHidden: true, pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublishedId == 0
                    ? null
                    : (bool?) (searchModel.SearchPublishedId == 1));

            //prepare grid model
            var model = new HotelTypeListModel().PrepareToGrid(searchModel, hotelTypes, () =>
            {
                return hotelTypes.Select(hotelType =>
                {
                    var hotelTypeModel = hotelType.ToModel<HotelTypeModel>();

                    hotelTypeModel.SeName = _urlRecordService.GetSeName(hotelType, 0, true, false);
                    
                    return hotelTypeModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare hotelType model
        /// </summary>
        /// <param name="model">HotelType model</param>
        /// <param name="hotelType">HotelType</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>HotelType model</returns>
        public virtual HotelTypeModel PrepareHotelTypeModel(HotelTypeModel model, HotelType hotelType, bool excludeProperties = false)
        {
            Action<HotelTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (hotelType != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = hotelType.ToModel<HotelTypeModel>();
                    model.SeName = _urlRecordService.GetSeName(hotelType, 0, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(hotelType, entity => entity.Name, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(hotelType, languageId, false, false);
                };
            }

            //set default values for the new model
            if (hotelType == null)
            {
                model.IsActive = true;
            }


            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
    }
}
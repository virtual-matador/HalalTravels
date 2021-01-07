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
    /// Represents the pricingModel model factory implementation
    /// </summary>
    public partial class PricingModelModelFactory : IPricingModelModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IPricingModelService _pricingModelService;
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

        public PricingModelModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            IPricingModelService pricingModelService,
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
            _pricingModelService = pricingModelService;
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
        /// Prepare pricingModel search model
        /// </summary>
        /// <param name="searchModel">PricingModel search model</param>
        /// <returns>PricingModel search model</returns>
        public virtual PricingModelSearchModel PreparePricingModelSearchModel(PricingModelSearchModel searchModel)
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
        /// Prepare paged pricingModel list model
        /// </summary>
        /// <param name="searchModel">PricingModel search model</param>
        /// <returns>PricingModel list model</returns>
        public virtual PricingModelListModel PreparePricingModelListModel(PricingModelSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var pricingModels = _pricingModelService.SearchPricingModels(showHidden: true, pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublishedId == 0
                    ? null
                    : (bool?) (searchModel.SearchPublishedId == 1));

            //prepare grid model
            var model = new PricingModelListModel().PrepareToGrid(searchModel, pricingModels, () =>
            {
                return pricingModels.Select(pricingModel =>
                {
                    var pricingModelModel = pricingModel.ToModel<PricingModelModel>();
                    
                    return pricingModelModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare pricingModel model
        /// </summary>
        /// <param name="model">PricingModel model</param>
        /// <param name="pricingModel">PricingModel</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>PricingModel model</returns>
        public virtual PricingModelModel PreparePricingModelModel(PricingModelModel model, PricingModel pricingModel, bool excludeProperties = false)
        {
            Action<PricingModelLocalizedModel, int> localizedModelConfiguration = null;

            if (pricingModel != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = pricingModel.ToModel<PricingModelModel>();
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(pricingModel, entity => entity.Name, languageId, false, false);
                };
            }

            //set default values for the new model
            if (pricingModel == null)
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
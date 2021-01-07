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
    /// Represents the chain model factory implementation
    /// </summary>
    public partial class ChainModelFactory : IChainModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IChainService _chainService;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly TaxSettings _taxSettings;

        protected readonly ICountryService _countryService;
        protected readonly IProvinceService _provinceService;
        protected readonly ICityService _cityService;

        #endregion

        #region Ctor

        public ChainModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            IChainService chainService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IPictureService pictureService,
            TaxSettings taxSettings, 
            ICountryService countryService, 
            IProvinceService provinceService, 
            ICityService cityService,
            IUrlRecordService urlRecordService)
        {
            _currencySettings = currencySettings;
            _aclSupportedModelFactory = aclSupportedModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _chainService = chainService;
            _currencyService = currencyService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _taxSettings = taxSettings;
            _pictureService = pictureService;

            _countryService = countryService;
            _provinceService = provinceService;
            _cityService = cityService;
            _urlRecordService = urlRecordService;
        }

        #endregion

        /// <summary>
        /// Prepare chain search model
        /// </summary>
        /// <param name="searchModel">Chain search model</param>
        /// <returns>Chain search model</returns>
        public virtual ChainSearchModel PrepareChainSearchModel(ChainSearchModel searchModel)
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
        /// Prepare paged chain list model
        /// </summary>
        /// <param name="searchModel">Chain search model</param>
        /// <returns>Chain list model</returns>
        public virtual ChainListModel PrepareChainListModel(ChainSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var chains = _chainService.SearchChains(showHidden: true, pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublishedId == 0
                    ? null
                    : (bool?) (searchModel.SearchPublishedId == 1));

            //prepare grid model
            var model = new ChainListModel().PrepareToGrid(searchModel, chains, () =>
            {
                return chains.Select(chain =>
                {
                    var chainModel = chain.ToModel<ChainModel>();

                    chainModel.SeName = _urlRecordService.GetSeName(chain, 0, true, false);
                    
                    return chainModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare chain model
        /// </summary>
        /// <param name="model">Chain model</param>
        /// <param name="chain">Chain</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Chain model</returns>
        public virtual ChainModel PrepareChainModel(ChainModel model, Chain chain, bool excludeProperties = false)
        {
            Action<ChainLocalizedModel, int> localizedModelConfiguration = null;

            if (chain != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = chain.ToModel<ChainModel>();
                    model.SeName = _urlRecordService.GetSeName(chain, 0, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(chain, entity => entity.Name, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(chain, languageId, false, false);
                };
            }

            //set default values for the new model
            if (chain == null)
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
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
    /// Represents the contractType model factory implementation
    /// </summary>
    public partial class ContractTypeModelFactory : IContractTypeModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IContractTypeService _contractTypeService;
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

        public ContractTypeModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            IContractTypeService contractTypeService,
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
            _contractTypeService = contractTypeService;
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
        /// Prepare contractType search model
        /// </summary>
        /// <param name="searchModel">ContractType search model</param>
        /// <returns>ContractType search model</returns>
        public virtual ContractTypeSearchModel PrepareContractTypeSearchModel(ContractTypeSearchModel searchModel)
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
        /// Prepare paged contractType list model
        /// </summary>
        /// <param name="searchModel">ContractType search model</param>
        /// <returns>ContractType list model</returns>
        public virtual ContractTypeListModel PrepareContractTypeListModel(ContractTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var contractTypes = _contractTypeService.SearchContractTypes(showHidden: true, pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublishedId == 0
                    ? null
                    : (bool?) (searchModel.SearchPublishedId == 1));

            //prepare grid model
            var model = new ContractTypeListModel().PrepareToGrid(searchModel, contractTypes, () =>
            {
                return contractTypes.Select(contractType =>
                {
                    var contractTypeModel = contractType.ToModel<ContractTypeModel>();

                    contractTypeModel.SeName = _urlRecordService.GetSeName(contractType, 0, true, false);
                    
                    return contractTypeModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare contractType model
        /// </summary>
        /// <param name="model">ContractType model</param>
        /// <param name="contractType">ContractType</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>ContractType model</returns>
        public virtual ContractTypeModel PrepareContractTypeModel(ContractTypeModel model, ContractType contractType, bool excludeProperties = false)
        {
            Action<ContractTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (contractType != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = contractType.ToModel<ContractTypeModel>();
                    model.SeName = _urlRecordService.GetSeName(contractType, 0, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(contractType, entity => entity.Name, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(contractType, languageId, false, false);
                };
            }

            //set default values for the new model
            if (contractType == null)
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
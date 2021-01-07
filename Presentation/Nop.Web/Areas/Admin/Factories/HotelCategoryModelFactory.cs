using System;
using System.Collections.Generic;
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
    /// Represents the category model factory implementation
    /// </summary>
    public partial class HotelCategoryModelFactory : IHotelCategoryModelFactory
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICategoryService _categoryService;
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
        protected readonly IHotelService _hotelService;

        #endregion

        #region Ctor

        public HotelCategoryModelFactory(CurrencySettings currencySettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IPictureService pictureService,
            TaxSettings taxSettings, 
            ICountryService countryService, 
            IProvinceService provinceService, 
            ICityService cityService,
            IHotelService hotelService,
            IUrlRecordService urlRecordService)
        {
            _currencySettings = currencySettings;
            _aclSupportedModelFactory = aclSupportedModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _taxSettings = taxSettings;
            _pictureService = pictureService;

            _countryService = countryService;
            _provinceService = provinceService;
            _cityService = cityService;
            _hotelService = hotelService;
            _urlRecordService = urlRecordService;
        }

        #endregion

        /// <summary>
        /// Prepare category search model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category search model</returns>
        public virtual CategorySearchModel PrepareCategorySearchModel(CategorySearchModel searchModel)
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
        /// Prepare paged category list model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category list model</returns>
        public virtual CategoryListModel PrepareCategoryListModel(CategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var categorys = _categoryService.SearchCategories(showHidden: true, pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                overridePublished: searchModel.SearchPublishedId == 0
                    ? null
                    : (bool?) (searchModel.SearchPublishedId == 1));

            //prepare grid model
            var model = new CategoryListModel().PrepareToGrid(searchModel, categorys, () =>
            {
                return categorys.Select(category =>
                {
                    var categoryModel = category.ToModel<CategoryModel>();

                    categoryModel.SeName = _urlRecordService.GetSeName(category, 0, true, false);
                    
                    return categoryModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare category model
        /// </summary>
        /// <param name="model">Category model</param>
        /// <param name="category">Category</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Category model</returns>
        public virtual CategoryModel PrepareCategoryModel(CategoryModel model, Category category, bool excludeProperties = false)
        {
            Action<CategoryLocalizedModel, int> localizedModelConfiguration = null;

            if (category != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = category.ToModel<CategoryModel>();
                    model.SeName = _urlRecordService.GetSeName(category, 0, true, false);
                }
                
                PrepareCategoryHotelSearchModel(model.CategoryHotelSearchModel, category);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(category, entity => entity.Name, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(category, languageId, false, false);
                };
            }

            //set default values for the new model
            if (category == null)
            {
                model.IsActive = true;
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        
        /// <summary>
        /// Prepare paged category hotel list model
        /// </summary>
        /// <param name="searchModel">Category hotel search model</param>
        /// <param name="category">Category</param>
        /// <returns>Category hotel list model</returns>
        public virtual CategoryHotelListModel PrepareCategoryHotelListModel(CategoryHotelSearchModel searchModel, Category category)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            //get hotel categories
            var hotelCategories = _categoryService.GetHotelCategoriesByCategoryId(category.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model
            var model = new CategoryHotelListModel().PrepareToGrid(searchModel, hotelCategories, () =>
            {
                return hotelCategories.Select(hotelCategory =>
                {
                    //fill in model values from the entity
                    var categoryHotelModel = hotelCategory.ToModel<CategoryHotelModel>();

                    //fill in additional values (not existing in the entity)
                    categoryHotelModel.HotelName = _hotelService.GetHotelById(hotelCategory.HotelId)?.Name;

                    return categoryHotelModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare hotel search model to add to the category
        /// </summary>
        /// <param name="searchModel">Hotel search model to add to the category</param>
        /// <returns>Hotel search model to add to the category</returns>
        public virtual AddHotelToCategorySearchModel PrepareAddHotelToCategorySearchModel(AddHotelToCategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available categories
            _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories);

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(searchModel.AvailableStores);

            //prepare available vendors
            _baseAdminModelFactory.PrepareVendors(searchModel.AvailableVendors);

            //prepare available hotel types
            _baseAdminModelFactory.PrepareHotelTypes(searchModel.AvailableHotelTypes);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged hotel list model to add to the category
        /// </summary>
        /// <param name="searchModel">Hotel search model to add to the category</param>
        /// <returns>Hotel list model to add to the category</returns>
        public virtual AddHotelToCategoryListModel PrepareAddHotelToCategoryListModel(AddHotelToCategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get hotels
            var hotels = _hotelService.SearchHotels(showHidden: true,
                categoryIds: new List<int> { searchModel.SearchCategoryId },
                hotelTypeIds: searchModel.SearchHotelTypeId > 0 ? new List<int> { searchModel.SearchHotelTypeId } : null,
                keywords: searchModel.SearchHotelName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model
            var model = new AddHotelToCategoryListModel().PrepareToGrid(searchModel, hotels, () =>
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
        
        public virtual CategoryHotelSearchModel PrepareCategoryHotelSearchModel(CategoryHotelSearchModel searchModel, Category category)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            searchModel.CategoryId = category.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Hotels;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.ExportImport;
using Nop.Services.Hotels;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using ICategoryService = Nop.Services.Hotels.ICategoryService;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class HotelCategoryController : BaseAdminController
    {
        #region Fields

        private readonly ICategoryService _hotelCategoryService;
        private readonly IHotelCategoryModelFactory _hotelCategoryModelFactory;
        private readonly IHotelService _hotelService;


        private readonly IDownloadService _downloadService;
        private readonly IExportManager _exportManager;
        private readonly IImportManager _importManager;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly INopFileProvider _fileProvider;
        private readonly INotificationService _notificationService;
        private readonly IPdfService _pdfService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IShippingService _shippingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;

        #endregion

        #region Ctor

        public HotelCategoryController(ICategoryService hotelCategoryService,
            IHotelCategoryModelFactory hotelCategoryModelFactory, 
            IHotelService hotelService,

            IDownloadService downloadService,
            IExportManager exportManager,
            IImportManager importManager,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            INopFileProvider fileProvider,
            INotificationService notificationService,
            IPdfService pdfService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingService settingService,
            IShippingService shippingService,
            IShoppingCartService shoppingCartService,
            ISpecificationAttributeService specificationAttributeService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            ICustomerService customerService,
            IBaseAdminModelFactory baseAdminModelFactory)
        {
            _hotelCategoryService = hotelCategoryService;
            _hotelCategoryModelFactory = hotelCategoryModelFactory;
            _hotelService = hotelService;
            _downloadService = downloadService;
            _exportManager = exportManager;
            _importManager = importManager;
            _languageService = languageService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _fileProvider = fileProvider;
            _notificationService = notificationService;
            _pdfService = pdfService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _shippingService = shippingService;
            _shoppingCartService = shoppingCartService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
            _customerService = customerService;
            _baseAdminModelFactory = baseAdminModelFactory;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(Category hotelCategory, CategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(hotelCategory,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(hotelCategory, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(hotelCategory, seName, localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        #region HotelCategory list / create / edit / delete

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedView();

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareCategorySearchModel(new CategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult CategoryList(CategorySearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareCategoryListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            /*if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
                return AccessDeniedView();*/

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareCategoryModel(new CategoryModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //hotelCategory
                var hotelCategory = model.ToEntity<Category>();

                _hotelCategoryService.InsertCategory(hotelCategory);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotelCategory, model.SeName, hotelCategory.Name, true);
                _urlRecordService.SaveSlug(hotelCategory, model.SeName, 0);

                //locales
                UpdateLocales(hotelCategory, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.HotelCategory.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotelCategory.Id });
            }

            //prepare model
            model = _hotelCategoryModelFactory.PrepareCategoryModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedView();

            //try to get a hotelCategory with the specified id
            var hotelCategory = _hotelCategoryService.GetCategoryById(id);
            if (hotelCategory == null)
                return RedirectToAction("List");

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareCategoryModel(null, hotelCategory);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CategoryModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedView();

            //try to get a hotelCategory with the specified id
            var hotelCategory = _hotelCategoryService.GetCategoryById(model.Id);
            if (hotelCategory == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //hotelCategory
                hotelCategory = model.ToEntity(hotelCategory);

                _hotelCategoryService.UpdateCategory(hotelCategory);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotelCategory, model.SeName, hotelCategory.Name, true);
                _urlRecordService.SaveSlug(hotelCategory, model.SeName, 0);

                //locales
                UpdateLocales(hotelCategory, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.HotelCategory.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotelCategory.Id });
            }

            //prepare model
            model = _hotelCategoryModelFactory.PrepareCategoryModel(model, hotelCategory, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedView();

            //try to get a hotelCategory with the specified id
            var hotelCategory = _hotelCategoryService.GetCategoryById(id);
            if (hotelCategory == null)
                return RedirectToAction("List");

            _hotelCategoryService.DeleteCategory(hotelCategory);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Category.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelCategories))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _hotelCategoryService.DeleteCategories(_hotelCategoryService.GetCategoriesByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Hotels

        [HttpPost]
        public virtual IActionResult HotelList(CategoryHotelSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedDataTablesJson();

            //try to get a category with the specified id
            var category = _hotelCategoryService.GetCategoryById(searchModel.CategoryId)
                ?? throw new ArgumentException("No category found with the specified id");

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareCategoryHotelListModel(searchModel, category);

            return Json(model);
        }

        public virtual IActionResult HotelUpdate(CategoryHotelModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //try to get a hotel category with the specified id
            var hotelCategory = _hotelCategoryService.GetHotelCategoryById(model.Id)
                ?? throw new ArgumentException("No hotel category mapping found with the specified id");

            //fill entity from hotel
            hotelCategory = model.ToEntity(hotelCategory);
            _hotelCategoryService.UpdateHotelCategory(hotelCategory);

            return new NullJsonResult();
        }

        public virtual IActionResult HotelDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //try to get a hotel category with the specified id
            var hotelCategory = _hotelCategoryService.GetHotelCategoryById(id)
                ?? throw new ArgumentException("No hotel category mapping found with the specified id", nameof(id));

            _hotelCategoryService.DeleteHotelCategory(hotelCategory);

            return new NullJsonResult();
        }

        public virtual IActionResult HotelAddPopup(int categoryId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareAddHotelToCategorySearchModel(new AddHotelToCategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelAddPopupList(AddHotelToCategorySearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelCategoryModelFactory.PrepareAddHotelToCategoryListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult HotelAddPopup(AddHotelToCategoryModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //get selected hotels
            var selectedHotels = _hotelService.GetHotelsByIds(model.SelectedHotelIds.ToArray());
            if (selectedHotels.Any())
            {
                var existingHotelCategories = _hotelCategoryService.GetHotelCategoriesByCategoryId(model.CategoryId, showHidden: true);
                foreach (var hotel in selectedHotels)
                {
                    //whether hotel category with such parameters already exists
                    if (_hotelCategoryService.FindHotelCategory(existingHotelCategories, hotel.Id, model.CategoryId) != null)
                        continue;

                    //insert the new hotel category mapping
                    _hotelCategoryService.InsertHotelCategory(new HotelCategory
                    {
                        CategoryId = model.CategoryId,
                        HotelId = hotel.Id
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View(new AddHotelToCategorySearchModel());
        }

        #endregion

        #endregion
    }
}

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
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class HotelTypeController : BaseAdminController
    {
        #region Fields

        private readonly IHotelTypeService _hotelTypeService;
        private readonly IHotelTypeModelFactory _hotelTypeModelFactory;


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

        public HotelTypeController(IHotelTypeService hotelTypeService,
            IHotelTypeModelFactory hotelTypeModelFactory,

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
            _hotelTypeService = hotelTypeService;
            _hotelTypeModelFactory = hotelTypeModelFactory;
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

        protected virtual void UpdateLocales(HotelType hotelType, HotelTypeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(hotelType,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(hotelType, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(hotelType, seName, localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        #region HotelType list / create / edit / delete

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedView();

            //prepare model
            var model = _hotelTypeModelFactory.PrepareHotelTypeSearchModel(new HotelTypeSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelTypeList(HotelTypeSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelTypeModelFactory.PrepareHotelTypeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            /*if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
                return AccessDeniedView();*/

            //prepare model
            var model = _hotelTypeModelFactory.PrepareHotelTypeModel(new HotelTypeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(HotelTypeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //hotelType
                var hotelType = model.ToEntity<HotelType>();

                _hotelTypeService.InsertHotelType(hotelType);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotelType, model.SeName, hotelType.Name, true);
                _urlRecordService.SaveSlug(hotelType, model.SeName, 0);

                //locales
                UpdateLocales(hotelType, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.HotelType.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotelType.Id });
            }

            //prepare model
            model = _hotelTypeModelFactory.PrepareHotelTypeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedView();

            //try to get a hotelType with the specified id
            var hotelType = _hotelTypeService.GetHotelTypeById(id);
            if (hotelType == null)
                return RedirectToAction("List");

            //prepare model
            var model = _hotelTypeModelFactory.PrepareHotelTypeModel(null, hotelType);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(HotelTypeModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedView();

            //try to get a hotelType with the specified id
            var hotelType = _hotelTypeService.GetHotelTypeById(model.Id);
            if (hotelType == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //hotelType
                hotelType = model.ToEntity(hotelType);

                _hotelTypeService.UpdateHotelType(hotelType);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotelType, model.SeName, hotelType.Name, true);
                _urlRecordService.SaveSlug(hotelType, model.SeName, 0);

                //locales
                UpdateLocales(hotelType, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.HotelType.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotelType.Id });
            }

            //prepare model
            model = _hotelTypeModelFactory.PrepareHotelTypeModel(model, hotelType, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedView();

            //try to get a hotelType with the specified id
            var hotelType = _hotelTypeService.GetHotelTypeById(id);
            if (hotelType == null)
                return RedirectToAction("List");

            _hotelTypeService.DeleteHotelType(hotelType);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.HotelType.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTypes))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _hotelTypeService.DeleteHotelTypes(_hotelTypeService.GetHotelTypesByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}

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
    public partial class ContractTypeController : BaseAdminController
    {
        #region Fields

        private readonly IContractTypeService _contractTypeService;
        private readonly IContractTypeModelFactory _contractTypeModelFactory;


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

        public ContractTypeController(IContractTypeService contractTypeService,
            IContractTypeModelFactory contractTypeModelFactory,

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
            _contractTypeService = contractTypeService;
            _contractTypeModelFactory = contractTypeModelFactory;
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

        protected virtual void UpdateLocales(ContractType contractType, ContractTypeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(contractType,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(contractType, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(contractType, seName, localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        #region ContractType list / create / edit / delete

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedView();

            //prepare model
            var model = _contractTypeModelFactory.PrepareContractTypeSearchModel(new ContractTypeSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ContractTypeList(ContractTypeSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _contractTypeModelFactory.PrepareContractTypeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            /*if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
                return AccessDeniedView();*/

            //prepare model
            var model = _contractTypeModelFactory.PrepareContractTypeModel(new ContractTypeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractTypeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //contractType
                var contractType = model.ToEntity<ContractType>();

                _contractTypeService.InsertContractType(contractType);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(contractType, model.SeName, contractType.Name, true);
                _urlRecordService.SaveSlug(contractType, model.SeName, 0);

                //locales
                UpdateLocales(contractType, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.ContractType.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = contractType.Id });
            }

            //prepare model
            model = _contractTypeModelFactory.PrepareContractTypeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedView();

            //try to get a contractType with the specified id
            var contractType = _contractTypeService.GetContractTypeById(id);
            if (contractType == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractTypeModelFactory.PrepareContractTypeModel(null, contractType);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ContractTypeModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedView();

            //try to get a contractType with the specified id
            var contractType = _contractTypeService.GetContractTypeById(model.Id);
            if (contractType == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //contractType
                contractType = model.ToEntity(contractType);

                _contractTypeService.UpdateContractType(contractType);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(contractType, model.SeName, contractType.Name, true);
                _urlRecordService.SaveSlug(contractType, model.SeName, 0);

                //locales
                UpdateLocales(contractType, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.ContractType.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = contractType.Id });
            }

            //prepare model
            model = _contractTypeModelFactory.PrepareContractTypeModel(model, contractType, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedView();

            //try to get a contractType with the specified id
            var contractType = _contractTypeService.GetContractTypeById(id);
            if (contractType == null)
                return RedirectToAction("List");

            _contractTypeService.DeleteContractType(contractType);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.ContractType.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractTypes))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _contractTypeService.DeleteContractTypes(_contractTypeService.GetContractTypesByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}

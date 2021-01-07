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
    public partial class ChainController : BaseAdminController
    {
        #region Fields

        private readonly IChainService _chainService;
        private readonly IChainModelFactory _chainModelFactory;


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

        public ChainController(IChainService chainService,
            IChainModelFactory chainModelFactory,

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
            _chainService = chainService;
            _chainModelFactory = chainModelFactory;
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

        protected virtual void UpdateLocales(Chain chain, ChainModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(chain,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(chain, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(chain, seName, localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        #region Chain list / create / edit / delete

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedView();

            //prepare model
            var model = _chainModelFactory.PrepareChainSearchModel(new ChainSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ChainList(ChainSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _chainModelFactory.PrepareChainListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            /*if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
                return AccessDeniedView();*/

            //prepare model
            var model = _chainModelFactory.PrepareChainModel(new ChainModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ChainModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //chain
                var chain = model.ToEntity<Chain>();

                _chainService.InsertChain(chain);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(chain, model.SeName, chain.Name, true);
                _urlRecordService.SaveSlug(chain, model.SeName, 0);

                //locales
                UpdateLocales(chain, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Chain.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = chain.Id });
            }

            //prepare model
            model = _chainModelFactory.PrepareChainModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedView();

            //try to get a chain with the specified id
            var chain = _chainService.GetChainById(id);
            if (chain == null)
                return RedirectToAction("List");

            //prepare model
            var model = _chainModelFactory.PrepareChainModel(null, chain);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ChainModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedView();

            //try to get a chain with the specified id
            var chain = _chainService.GetChainById(model.Id);
            if (chain == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //chain
                chain = model.ToEntity(chain);

                _chainService.UpdateChain(chain);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(chain, model.SeName, chain.Name, true);
                _urlRecordService.SaveSlug(chain, model.SeName, 0);

                //locales
                UpdateLocales(chain, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Chain.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = chain.Id });
            }

            //prepare model
            model = _chainModelFactory.PrepareChainModel(model, chain, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedView();

            //try to get a chain with the specified id
            var chain = _chainService.GetChainById(id);
            if (chain == null)
                return RedirectToAction("List");

            _chainService.DeleteChain(chain);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Chain.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageChains))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _chainService.DeleteChains(_chainService.GetChainsByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}

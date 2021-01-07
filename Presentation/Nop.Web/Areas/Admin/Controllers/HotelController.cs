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
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using ICategoryService = Nop.Services.Hotels.ICategoryService;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class HotelController : BaseAdminController
    {
        #region Fields

        private readonly IAclService _aclService;
        private readonly IHotelService _hotelService;
        private readonly IHotelTypeService _hotelTypeService;
        private readonly IChainService _chainService;
        private readonly IHotelModelFactory _hotelModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IHotelTagService _hotelTagService;
        private readonly IHotelPropertyService _hotelPropertyService;

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

        public HotelController(IAclService aclService,
            IHotelService hotelService,
            IHotelTypeService hotelTypeService,
            IChainService chainService,
            IHotelModelFactory hotelModelFactory,
            ICategoryService categoryService,
            IHotelTagService hotelTagService,
            IHotelPropertyService hotelPropertyService,

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
            _aclService = aclService;
            _hotelService = hotelService;
            _hotelTypeService = hotelTypeService;
            _chainService = chainService;
            _hotelModelFactory = hotelModelFactory;
            _categoryService = categoryService;
            _hotelTagService = hotelTagService;
            _hotelPropertyService = hotelPropertyService;

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

        protected virtual void UpdateLocales(Hotel hotel, HotelModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(hotel,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(hotel,
                    x => x.ShortDescription,
                    localized.ShortDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(hotel,
                    x => x.LongDescription,
                    localized.LongDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(hotel,
                    x => x.AdminComment,
                    localized.AdminComment,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(hotel, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(hotel, seName, localized.LanguageId);
            }
        }

        protected virtual void UpdateLocales(HotelTag hotelTag, HotelTagModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(hotelTag,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                var seName = _urlRecordService.ValidateSeName(hotelTag, string.Empty, localized.Name, false);
                _urlRecordService.SaveSlug(hotelTag, seName, localized.LanguageId);
            }
        }
        
        protected virtual void UpdateLocales(HotelContact hotelContact, HotelContactModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(hotelContact,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
                
                _localizedEntityService.SaveLocalizedValue(hotelContact,
                    x => x.Surename,
                    localized.Surename,
                    localized.LanguageId);
                
                _localizedEntityService.SaveLocalizedValue(hotelContact,
                    x => x.Position,
                    localized.Position,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdatePictureSeoNames(Hotel hotel)
        {
            foreach (var pp in _hotelService.GetHotelPicturesByHotelId(hotel.Id))
                _pictureService.SetSeoFilename(pp.PictureId, _pictureService.GetPictureSeName(hotel.Name));
        }

        protected virtual void SaveHotelAcl(Hotel hotel, HotelModel model)
        {
            hotel.SubjectToAcl = model.SelectedCustomerRoleIds.Any();
            _hotelService.UpdateHotel(hotel);

            var existingAclRecords = _aclService.GetAclRecords(hotel);
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            foreach (var customerRole in allCustomerRoles)
            {
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                {
                    //new role
                    if (existingAclRecords.Count(acl => acl.CustomerRoleId == customerRole.Id) == 0)
                        _aclService.InsertAclRecord(hotel, customerRole.Id);
                }
                else
                {
                    //remove role
                    var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CustomerRoleId == customerRole.Id);
                    if (aclRecordToDelete != null)
                        _aclService.DeleteAclRecord(aclRecordToDelete);
                }
            }
        }

        protected virtual void SaveCategoryMappings(Hotel hotel, HotelModel model)
        {
            var existingHotelCategories = _categoryService.GetHotelCategoriesByHotelId(hotel.Id, true);

            //delete categories
            foreach (var existingHotelCategory in existingHotelCategories)
                if (!model.SelectedCategoryIds.Contains(existingHotelCategory.CategoryId))
                    _categoryService.DeleteHotelCategory(existingHotelCategory);

            //add categories
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (_categoryService.FindHotelCategory(existingHotelCategories, hotel.Id, categoryId) == null)
                {
                    _categoryService.InsertHotelCategory(new HotelCategory
                    {
                        HotelId = hotel.Id,
                        CategoryId = categoryId
                    });
                }
            }
        }
        
        protected virtual void SaveLimitedToCountryMappings(Hotel hotel, HotelModel model)
        {
            hotel.LimitedToCountries = model.SelectedCountryLimitationIds.Any();
            var existingLimitations = _hotelService.GetHotelLimitedToCountriesByHotelId(hotel.Id, true);
            
            foreach (var existingLimitation in existingLimitations)
                if (!model.SelectedCountryLimitationIds.Contains(existingLimitation.CountryId))
                    _hotelService.DeleteHotelLimitedToCountry(existingLimitation);
            
            //add countries
            foreach (var countryId in model.SelectedCountryLimitationIds)
            {
                if (_hotelService.FindHotelLimitedToCountry(existingLimitations, hotel.Id, countryId) == null)
                {
                    _hotelService.InsertHotelLimitedToCountry(new HotelLimitedToCountry
                    {
                        HotelId = hotel.Id,
                        CountryId = countryId
                    });
                }
            }
        }
        
        protected virtual void SaveHotelCityMappings(Hotel hotel, HotelModel model)
        {
            var mappedCities = _hotelService.GetHotelCityMappingsByHotelId(hotel.Id, true);

            foreach (var cityMapping in mappedCities)
            {
                if (!model.SelectedCityIds.Contains(cityMapping.CityId))
                    _hotelService.DeleteHotelCityMapping(cityMapping);
            }

            foreach (var cityId in model.SelectedCityIds)
            {
                if (_hotelService.FindHotelCityMapping(mappedCities, hotel.Id, cityId) == null)
                    _hotelService.InsertHotelCityMapping(new HotelCityMapping
                    {
                        HotelId = hotel.Id,
                        CityId = cityId
                    });
            }
        }

        protected virtual string[] ParseHotelTags(string hotelTags)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(hotelTags))
                return result.ToArray();

            var values = hotelTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in values)
                if (!string.IsNullOrEmpty(val.Trim()))
                    result.Add(val.Trim());

            return result.ToArray();
        }

        #endregion

        #region Methods

        #region Hotel list / create / edit / delete

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //prepare model
            var model = _hotelModelFactory.PrepareHotelSearchModel(new HotelSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelList(HotelSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelModelFactory.PrepareHotelListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            /*if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
                return AccessDeniedView();*/

            //prepare model
            var model = _hotelModelFactory.PrepareHotelModel(new HotelModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(HotelModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //hotel
                var hotel = model.ToEntity<Hotel>();

                _hotelService.InsertHotel(hotel);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotel, model.SeName, hotel.Name, true);
                _urlRecordService.SaveSlug(hotel, model.SeName, 0);

                //locales
                UpdateLocales(hotel, model);

                SaveCategoryMappings(hotel, model);
                
                SaveLimitedToCountryMappings(hotel, model);

                //ACL (customer roles)
                SaveHotelAcl(hotel, model);

                _hotelTagService.UpdateHotelTags(hotel, ParseHotelTags(model.HotelTags));

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Hotel.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotel.Id });
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(id);
            if (hotel == null)
                return RedirectToAction("List");
            
            //prepare model
            var model = _hotelModelFactory.PrepareHotelModel(null, hotel);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(HotelModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(model.Id);
            if (hotel == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                //hotel
                hotel = model.ToEntity(hotel);

                _hotelService.UpdateHotel(hotel);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(hotel, model.SeName, hotel.Name, true);
                _urlRecordService.SaveSlug(hotel, model.SeName, 0);

                //locales
                UpdateLocales(hotel, model);

                SaveCategoryMappings(hotel, model);
                
                SaveLimitedToCountryMappings(hotel, model);

                //ACL (customer roles)
                SaveHotelAcl(hotel, model);

                _hotelTagService.UpdateHotelTags(hotel, ParseHotelTags(model.HotelTags));

                //picture seo names
                UpdatePictureSeoNames(hotel);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Hotel.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hotel.Id });
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelModel(model, hotel, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(id);
            if (hotel == null)
                return RedirectToAction("List");

            _hotelService.DeleteHotel(hotel);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Hotel.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _hotelService.DeleteHotels(_hotelService.GetHotelsByIds(selectedIds.ToArray()).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Hotel pictures

        public virtual IActionResult HotelPictureAdd(int pictureId, int displayOrder, string overrideAltAttribute, string overrideTitleAttribute, int hotelId)
        {
            if (pictureId == 0)
                throw new ArgumentException();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(hotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            if (_hotelService.GetHotelPicturesByHotelId(hotelId).Any(p => p.PictureId == pictureId))
                return Json(new { Result = false });

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(pictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(hotel.Name));

            _hotelService.InsertHotelPicture(new HotelPicture
            {
                PictureId = pictureId,
                HotelId = hotelId,
                DisplayOrder = displayOrder
            });

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult HotelPictureList(HotelPictureSearchModel searchModel)
        {
            var hotel = _hotelService.GetHotelById(searchModel.HotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            //prepare model
            var model = _hotelModelFactory.PrepareHotelPictureListModel(searchModel, hotel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult HotelPictureUpdate(HotelPictureModel model)
        {
            //try to get a hotel picture with the specified id
            var hotelPicture = _hotelService.GetHotelPictureById(model.Id)
                ?? throw new ArgumentException("No hotel picture found with the specified id");

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(hotelPicture.PictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            hotelPicture.DisplayOrder = model.DisplayOrder;
            _hotelService.UpdateHotelPicture(hotelPicture);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult HotelPictureDelete(int id)
        {
            //try to get a hotel picture with the specified id
            var hotelPicture = _hotelService.GetHotelPictureById(id)
                ?? throw new ArgumentException("No hotel picture found with the specified id");

            var pictureId = hotelPicture.PictureId;
            _hotelService.DeleteHotelPicture(hotelPicture);

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(pictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }

        #endregion

        #region Related hotels

        [HttpPost]
        public virtual IActionResult RelatedHotelList(RelatedHotelSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedDataTablesJson();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(searchModel.HotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            //a vendor should have access only to his hotels
            //if (_workContext.CurrentVendor != null && hotel.VendorId != _workContext.CurrentVendor.Id)
            //    return Content("This is not your hotel");

            //prepare model
            var model = _hotelModelFactory.PrepareRelatedHotelListModel(searchModel, hotel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult RelatedHotelUpdate(RelatedHotelModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //try to get a related hotel with the specified id
            var relatedHotel = _hotelService.GetRelatedHotelById(model.Id)
                ?? throw new ArgumentException("No related hotel found with the specified id");

            //a vendor should have access only to his hotels
            //if (_workContext.CurrentVendor != null)
            //{
            //    var hotel = _hotelService.GetHotelById(relatedHotel.HotelId);
            //    if (hotel != null && hotel.VendorId != _workContext.CurrentVendor.Id)
            //        return Content("This is not your hotel");
            //}

            relatedHotel.DisplayOrder = model.DisplayOrder;
            _hotelService.UpdateRelatedHotel(relatedHotel);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult RelatedHotelDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //try to get a related hotel with the specified id
            var relatedHotel = _hotelService.GetRelatedHotelById(id)
                ?? throw new ArgumentException("No related hotel found with the specified id");

            var hotelId = relatedHotel.HotelId;

            //a vendor should have access only to his hotels
            //if (_workContext.CurrentVendor != null)
            //{
            //    var hotel = _hotelService.GetHotelById(hotelId);
            //    if (hotel != null && hotel.VendorId != _workContext.CurrentVendor.Id)
            //        return Content("This is not your hotel");
            //}

            _hotelService.DeleteRelatedHotel(relatedHotel);

            return new NullJsonResult();
        }

        public virtual IActionResult RelatedHotelAddPopup(int hotelId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            //prepare model
            var model = _hotelModelFactory.PrepareAddRelatedHotelSearchModel(new AddRelatedHotelSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult RelatedHotelAddPopupList(AddRelatedHotelSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelModelFactory.PrepareAddRelatedHotelListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult RelatedHotelAddPopup(AddRelatedHotelModel model)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotels))
            //    return AccessDeniedView();

            var selectedHotels = _hotelService.GetHotelsByIds(model.SelectedHotelIds.ToArray());
            if (selectedHotels.Any())
            {
                var existingRelatedHotels = _hotelService.GetRelatedHotelsByHotelId(model.HotelId, showHidden: true);
                foreach (var hotel in selectedHotels)
                {
                    //a vendor should have access only to his hotels
                    //if (_workContext.CurrentVendor != null && hotel.VendorId != _workContext.CurrentVendor.Id)
                    //    continue;

                    if (_hotelService.FindRelatedHotel(existingRelatedHotels, model.HotelId, hotel.Id) != null)
                        continue;

                    _hotelService.InsertRelatedHotel(new RelatedHotel
                    {
                        HotelId = model.HotelId,
                        RelatedHotelId = hotel.Id,
                        DisplayOrder = 1
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View(new AddRelatedHotelSearchModel());
        }

        #endregion

        #region Hotel tags

        public virtual IActionResult HotelTags()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedView();

            //prepare model
            var model = _hotelModelFactory.PrepareHotelTagSearchModel(new HotelTagSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelTags(HotelTagSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedDataTablesJson();

            //prepare model
            var model = _hotelModelFactory.PrepareHotelTagListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult HotelTagDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedView();

            //try to get a hotel tag with the specified id
            var tag = _hotelTagService.GetHotelTagById(id)
                ?? throw new ArgumentException("No hotel tag found with the specified id");

            _hotelTagService.DeleteHotelTag(tag);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotel.HotelTags.Deleted"));

            return RedirectToAction("HotelTags");
        }

        [HttpPost]
        public virtual IActionResult HotelTagsDelete(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                var tags = _hotelTagService.GetHotelTagsByIds(selectedIds.ToArray());
                _hotelTagService.DeleteHotelTags(tags);
            }

            return Json(new { Result = true });
        }

        public virtual IActionResult EditHotelTag(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedView();

            //try to get a hotel tag with the specified id
            var hotelTag = _hotelTagService.GetHotelTagById(id);
            if (hotelTag == null)
                return RedirectToAction("List");

            //prepare tag model
            var model = _hotelModelFactory.PrepareHotelTagModel(null, hotelTag);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult EditHotelTag(HotelTagModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageHotelTags))
            //    return AccessDeniedView();

            //try to get a hotel tag with the specified id
            var hotelTag = _hotelTagService.GetHotelTagById(model.Id);
            if (hotelTag == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                hotelTag.Name = model.Name;
                _hotelTagService.UpdateHotelTag(hotelTag);

                //locales
                UpdateLocales(hotelTag, model);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.HotelTags.Updated"));

                return continueEditing ? RedirectToAction("EditHotelTag", new { id = hotelTag.Id }) : RedirectToAction("HotelTags");
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelTagModel(model, hotelTag, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Contacts

        [HttpPost]
        public virtual IActionResult HotelContactList(HotelContactSearchModel searchModel)
        {
            var hotel = _hotelService.GetHotelById(searchModel.HotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            //prepare model
            var model = _hotelModelFactory.PrepareHotelContactListModel(searchModel, hotel);

            return Json(model);
        }

        public virtual IActionResult HotelContactCreatePopup(int hotelId)
        {
            var hotel = _hotelService.GetHotelById(hotelId);
            if (hotel == null)
                return RedirectToAction("List");

            //prepare model
            var model = _hotelModelFactory
                .PrepareHotelContactModel(new HotelContactModel(), hotel, null);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelContactCreatePopup(HotelContactModel model)
        {
            var hotel = _hotelService.GetHotelById(model.HotelId);
            if (hotel == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var hotelContact = model.ToEntity<HotelContact>();
                _hotelService.InsertHotelContact(hotelContact);

                UpdateLocales(hotelContact, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelContactModel(model, hotel, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult HotelContactEditPopup(int id)
        {
            var hotelContact = _hotelService.GetHotelContactById(id);
            if (hotelContact == null)
                return RedirectToAction("List");

            //try to get a checkout attribute with the specified id
            var hotel = _hotelService.GetHotelById(hotelContact.HotelId);
            if (hotel == null)
                return RedirectToAction("List");

            //prepare model
            var model = _hotelModelFactory.PrepareHotelContactModel(null, hotel, hotelContact);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelContactEditPopup(HotelContactModel model)
        {
            var hotelContact = _hotelService.GetHotelContactById(model.Id);
            if (hotelContact == null)
                return RedirectToAction("List");

            //try to get a checkout attribute with the specified id
            var hotel = _hotelService.GetHotelById(hotelContact.HotelId);
            if (hotel == null)
                return RedirectToAction("List");

            

            if (ModelState.IsValid)
            {
                hotelContact = model.ToEntity(hotelContact);
                _hotelService.UpdateHotelContact(hotelContact);

                UpdateLocales(hotelContact, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelContactModel(model, hotel, hotelContact, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult HotelContactDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAttributes))
                return AccessDeniedView();

            //try to get a checkout attribute value with the specified id
            var hotelContact = _hotelService.GetHotelContactById(id)
                ?? throw new ArgumentException("No contact found with the specified id", nameof(id));

            _hotelService.DeleteHotelContact(hotelContact);

            return new NullJsonResult();
        }

        #endregion
        
        #region Contract Documents

        public virtual IActionResult ContractDocumentAdd(int documentId, int displayOrder, string overrideAltAttribute, string overrideTitleAttribute, int hotelId)
        {
            if (documentId == 0)
                throw new ArgumentException();

            //try to get a hotel with the specified id
            var hotel = _hotelService.GetHotelById(hotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            if (_hotelService.GetHotelContractDocumentsByHotelId(hotelId).Any(p => p.DocumentId == documentId))
                return Json(new { Result = false });

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(documentId)
                ?? throw new ArgumentException("No document found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            _pictureService.SetSeoFilename(documentId, _pictureService.GetPictureSeName(hotel.Name));

            _hotelService.InsertHotelContractDocument(new HotelContractDocument
            {
                DocumentId = documentId,
                DocumentType = "pdf",
                HotelId = hotelId,
                DisplayOrder = displayOrder
            });

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ContractDocumentList(HotelContractDocumentSearchModel searchModel)
        {
            var hotel = _hotelService.GetHotelById(searchModel.HotelId)
                ?? throw new ArgumentException("No hotel found with the specified id");

            //prepare model
            var model = _hotelModelFactory.PrepareContractDocumentListModel(searchModel, hotel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult ContractDocumentUpdate(HotelContractDocumentModel model)
        {
            //try to get a hotel picture with the specified id
            var document = _hotelService.GetHotelContractDocumentById(model.Id)
                ?? throw new ArgumentException("No hotel contract document found with the specified id");

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(document.DocumentId)
                ?? throw new ArgumentException("No document found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            document.DisplayOrder = model.DisplayOrder;
            document.DocumentType = model.DocumentType;
            _hotelService.UpdateHotelContractDocument(document);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult ContractDocumentDelete(int id)
        {
            //try to get a hotel picture with the specified id
            var document = _hotelService.GetHotelContractDocumentById(id)
                ?? throw new ArgumentException("No hotel contract document found with the specified id");

            var documentId = document.DocumentId;
            _hotelService.DeleteHotelContractDocument(document);

            //try to get a picture with the specified id
            var picture = _pictureService.GetPictureById(documentId)
                ?? throw new ArgumentException("No document found with the specified id");

            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }

        #endregion

        #region Amenities

        public virtual IActionResult UpdateAmenities(int id)
        {

        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult UpdateAmenities(HotelModel model, bool continueEditing)
        {
            var hotel = _hotelService.GetHotelById(model.Id);
            if (hotel == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Hotels.Hotel.Amenities.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("UpdateAmenities", new { id = hotel.Id });
            }

            //prepare model
            model = _hotelModelFactory.PrepareHotelModel(model, hotel, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion
        
        public virtual IActionResult SetDefaultHotel(int hotelId, string returnUrl = "")
        {
            var hotel = _hotelService.GetHotelById(hotelId);
            _workContext.DefaultHotel = hotel;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = AreaNames.Admin });

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            return Redirect(returnUrl);
        }

        #endregion
    }
}

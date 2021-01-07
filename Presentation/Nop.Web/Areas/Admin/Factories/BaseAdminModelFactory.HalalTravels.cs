using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Infrastructure.Cache;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class BaseAdminModelFactory
    {
        #region Methods

        /// <summary>
        /// Prepare available Chains
        /// </summary>
        /// <param name="items">Category items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareChains(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableChainItems = GetChainList();
            foreach (var chainItem in availableChainItems)
            {
                items.Add(chainItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available hotel types
        /// </summary>
        /// <param name="items">HotelType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareHotelTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableHotelTypeItems = GetHotelTypeList();
            foreach (var hotelTypeItem in availableHotelTypeItems)
            {
                items.Add(hotelTypeItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public virtual void PrepareHotelCountries(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableCountryItems = GetHotelCountriesList();
            foreach (var countryItem in availableCountryItems)
            {
                items.Add(countryItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public virtual void PrepareHotelProvinces(IList<SelectListItem> items, int countryId, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableItems = GetHotelProvincesList(countryId);
            foreach (var item in availableItems)
            {
                items.Add(item);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public virtual void PrepareHotelCounties(IList<SelectListItem> items, int provinceId, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableItems = GetHotelCountiesList(provinceId);
            foreach (var item in availableItems)
            {
                items.Add(item);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
        
        public virtual void PrepareHotelCities(IList<SelectListItem> items, int countyId, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableItems = GetHotelCitiesList(countyId);
            foreach (var item in availableItems)
            {
                items.Add(item);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available categories
        /// </summary>
        /// <param name="items">Category items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareHotelCategories(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableCategoryItems = GetHotelCategoryList();
            foreach (var categoryItem in availableCategoryItems)
            {
                items.Add(categoryItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available pricing models
        /// </summary>
        /// <param name="items">HotelType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PreparePricingModels(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var availablePricingModelItems = GetPricingModelList();
            foreach (var pricingModelItem in availablePricingModelItems)
            {
                items.Add(pricingModelItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available contract types
        /// </summary>
        /// <param name="items">ContractType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareContractTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableContractTypeItems = GetContractTypeList();
            foreach (var contractTypeItem in availableContractTypeItems)
            {
                items.Add(contractTypeItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
            
        }

        public virtual void PrepareDepartments(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            var availableDepartments = GetDepartmentList();
            foreach (var departmentItem in availableDepartments)
            {
                items.Add(departmentItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
        
        #endregion

        #region Utilities

        /// <summary>
        /// Get chain list
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Chain list</returns>
        protected virtual List<SelectListItem> GetChainList(bool showHidden = true)
        {
            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(NopModelCacheDefaults.ChainsListKey, showHidden);
            var listItems = _staticCacheManager.Get(cacheKey, () =>
            {
                var chains = _chainService.GetAllChains(showHidden: showHidden);

                return chains.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Get hotel type list
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelType list</returns>
        protected virtual List<SelectListItem> GetHotelTypeList(bool showHidden = true)
        {
            var cacheKey = _cacheKeyService.PrepareKeyForDefaultCache(NopModelCacheDefaults.HotelTypesListKey, showHidden);
            var listItems = _staticCacheManager.Get(cacheKey, () =>
            {
                var chains = _hotelTypeService.GetAllHotelTypes(showHidden: showHidden);

                return chains.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual List<SelectListItem> GetHotelCountriesList(bool showHidden = true)
        {
            var countries = _hotelCountryService.GetAllCountries(showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in countries)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual List<SelectListItem> GetHotelProvincesList(int countryId, bool showHidden = true)
        {
            var provinces = _hotelProvinceService.GetProvincesByCountryId(countryId, showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in provinces)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual List<SelectListItem> GetHotelCountiesList(int provinceId, bool showHidden = true)
        {
            var cities = _hotelCountyService.GetCountiesByProvinceId(provinceId, showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in cities)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }
        
        protected virtual List<SelectListItem> GetHotelCitiesList(int countyId, bool showHidden = true)
        {
            var cities = _hotelCityService.GetCitiesByCountyId(countyId, showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in cities)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual List<SelectListItem> GetHotelCategoryList(bool showHidden = true)
        {
            var cities = _hotelCategoryService.GetAllCategories(showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in cities)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        protected virtual List<SelectListItem> GetPricingModelList(bool showHidden = true)
        {
            var listItems = _pricingModelService.GetAllPricingModels(showHidden: showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            
            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Get contract type list
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ContractType list</returns>
        protected virtual List<SelectListItem> GetContractTypeList(bool showHidden = true)
        {
            var listItems = _contractTypeService.GetAllContractTypes(showHidden: showHidden).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }
        
        protected virtual List<SelectListItem> GetDepartmentList(bool showHidden = true)
        {
            var departments = _departmentService.GetAllDepartments(showHidden)
                .Select(department => new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Id.ToString()
                }).ToList();

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in departments)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }
        
        #endregion
    }
}

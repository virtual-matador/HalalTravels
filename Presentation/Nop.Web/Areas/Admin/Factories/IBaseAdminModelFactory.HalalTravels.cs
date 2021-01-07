
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IBaseAdminModelFactory
    {
        /// <summary>
        /// Prepare available chains
        /// </summary>
        /// <param name="items">Chain items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        void PrepareChains(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);

        /// <summary>
        /// Prepare available hotel types
        /// </summary>
        /// <param name="items">HotelType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        void PrepareHotelTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);

        void PrepareHotelCountries(IList<SelectListItem> items, bool withSpecialDefaultItem = true,
            string defaultItemText = null);

        void PrepareHotelProvinces(IList<SelectListItem> items, int countryId, bool withSpecialDefaultItem = true,
            string defaultItemText = null);

        void PrepareHotelCounties(IList<SelectListItem> items, int provinceId, bool withSpecialDefaultItem = true, string defaultItemText = null);
        
        void PrepareHotelCities(IList<SelectListItem> items, int countyId, bool withSpecialDefaultItem = true, string defaultItemText = null);

        /// <summary>
        /// Prepare available categories
        /// </summary>
        /// <param name="items">Category items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        void PrepareHotelCategories(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);
        
        /// <summary>
        /// Prepare available pricing models
        /// </summary>
        /// <param name="items">HotelType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        void PreparePricingModels(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);
        
        /// <summary>
        /// Prepare available contract types
        /// </summary>
        /// <param name="items">ContractType items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        void PrepareContractTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);
        
        void PrepareDepartments(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null);
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel contact model
    /// </summary>
    public partial class HotelContactModel : BaseNopEntityModel, ILocalizedModel<HotelContactLocalizedModel>
    {
        #region Ctor

        public HotelContactModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<HotelContactLocalizedModel>();
            AvailableDepartments = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.HotelId")]
        public int HotelId { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.DepartmentId")]
        public int DepartmentId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Name")]
        public string Name { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Surename")]
        public string Surename { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Position")]
        public string Position { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Email")]
        public string Email { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.MobileNo")]
        public string MobileNo { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.OfficePhones")]
        public string OfficePhones { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.IsDefault")]
        public bool IsDefault { get; set; }

        public IList<HotelContactLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelContact.Fields.PageSize")]
        public int PageSize { get; set; }
        
        public IList<SelectListItem> AvailableDepartments { get; set; }
        
        public string DepartmentName { get; set; }

        #endregion
    }

    public partial class HotelContactLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelContact.Fields.Name")]
        public string Name { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Surename")]
        public string Surename { get; set; }
        
        [NopResourceDisplayName("Admin.Hotels.Hotel.HotelContact.Fields.Position")]
        public string Position { get; set; }
    }
}

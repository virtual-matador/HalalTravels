using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel type model
    /// </summary>
    public partial class HotelTypeModel : BaseNopEntityModel, ILocalizedModel<HotelTypeLocalizedModel>, ISlugSupported
    {
        #region Ctor

        public HotelTypeModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<HotelTypeLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.HotelTypeFields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelTypes.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelType.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<HotelTypeLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelType.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelType.Fields.SeName")]
        public string SeName { get; set; }

        #endregion
    }

    public partial class HotelTypeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelType.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelType.Fields.SeName")]
        public string SeName { get; set; }
    }
}

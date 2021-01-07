using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a product tag model
    /// </summary>
    public partial class HotelTagModel : BaseNopEntityModel, ILocalizedModel<HotelTagLocalizedModel>
    {
        #region Ctor

        public HotelTagModel()
        {
            Locales = new List<HotelTagLocalizedModel>();
        }
        
        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.HotelTags.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelTags.Fields.HotelCount")]
        public int HotelCount { get; set; }

        public IList<HotelTagLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class HotelTagLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.HotelTags.Fields.Name")]
        public string Name { get; set; }
    }
}
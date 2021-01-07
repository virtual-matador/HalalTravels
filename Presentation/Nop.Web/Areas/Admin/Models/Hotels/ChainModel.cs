using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a chain model
    /// </summary>
    public partial class ChainModel : BaseNopEntityModel, ILocalizedModel<ChainLocalizedModel>, ISlugSupported
    {
        #region Ctor

        public ChainModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<ChainLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ChainLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.SeName")]
        public string SeName { get; set; }

        #endregion
    }

    public partial class ChainLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Chain.Fields.SeName")]
        public string SeName { get; set; }
    }
}

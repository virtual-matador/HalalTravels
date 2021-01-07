using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a pricing model model
    /// </summary>
    public partial class PricingModelModel : BaseNopEntityModel, ILocalizedModel<PricingModelLocalizedModel>
    {
        #region Ctor

        public PricingModelModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<PricingModelLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.PricingModelFields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.PricingModels.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.PricingModel.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<PricingModelLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.PricingModel.Fields.PageSize")]
        public int PageSize { get; set; }

        #endregion
    }

    public partial class PricingModelLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.PricingModel.Fields.Name")]
        public string Name { get; set; }
    }
}

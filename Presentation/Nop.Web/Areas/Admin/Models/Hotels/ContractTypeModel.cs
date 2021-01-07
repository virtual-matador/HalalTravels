using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel type model
    /// </summary>
    public partial class ContractTypeModel : BaseNopEntityModel, ILocalizedModel<ContractTypeLocalizedModel>, ISlugSupported
    {
        #region Ctor

        public ContractTypeModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<ContractTypeLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.ContractTypeFields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractTypes.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ContractTypeLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.Fields.SeName")]
        public string SeName { get; set; }

        #endregion
    }

    public partial class ContractTypeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.ContractType.Fields.SeName")]
        public string SeName { get; set; }
    }
}

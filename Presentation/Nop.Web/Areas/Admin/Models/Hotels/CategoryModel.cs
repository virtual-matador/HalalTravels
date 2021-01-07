using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a chain model
    /// </summary>
    public partial class CategoryModel : BaseNopEntityModel, ILocalizedModel<CategoryLocalizedModel>, ISlugSupported
    {
        #region Ctor

        public CategoryModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<CategoryLocalizedModel>();

            CategoryHotelSearchModel = new CategoryHotelSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<CategoryLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.SeName")]
        public string SeName { get; set; }

        public CategoryHotelSearchModel CategoryHotelSearchModel { get; set; }

        #endregion
    }

    public partial class CategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Category.Fields.SeName")]
        public string SeName { get; set; }
    }
}

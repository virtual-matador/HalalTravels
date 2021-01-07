using System.Collections.Generic;
using Nop.Core.Domain.Seo;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a department model
    /// </summary>
    public partial class DepartmentModel : BaseNopEntityModel, ILocalizedModel<DepartmentLocalizedModel>
    {
        #region Ctor

        public DepartmentModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<DepartmentLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Hotels.DepartmentFields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Departments.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Department.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<DepartmentLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Department.Fields.PageSize")]
        public int PageSize { get; set; }

        #endregion
    }

    public partial class DepartmentLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Department.Fields.Name")]
        public string Name { get; set; }
    }
}

using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the department model factory
    /// </summary>
    public partial interface IDepartmentModelFactory
    {
        /// <summary>
        /// Prepare department search model
        /// </summary>
        /// <param name="searchModel">Department search model</param>
        /// <returns>Department search model</returns>
        DepartmentSearchModel PrepareDepartmentSearchModel(DepartmentSearchModel searchModel);

        /// <summary>
        /// Prepare paged department list model
        /// </summary>
        /// <param name="searchModel">Department search model</param>
        /// <returns>Department list model</returns>
        DepartmentListModel PrepareDepartmentListModel(DepartmentSearchModel searchModel);

        /// <summary>
        /// Prepare department model
        /// </summary>
        /// <param name="model">Department model</param>
        /// <param name="department">Department</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Department model</returns>
        DepartmentModel PrepareDepartmentModel(DepartmentModel model, Department department, bool excludeProperties = false);
    }
}

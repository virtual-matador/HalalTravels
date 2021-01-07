using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Department service
    /// </summary>
    public partial interface IDepartmentService
    {
        #region Departments

        /// <summary>
        /// Delete a department
        /// </summary>
        /// <param name="department">Department</param>
        void DeleteDepartment(Department department);

        /// <summary>
        /// Delete departments
        /// </summary>
        /// <param name="departments">Departments</param>
        void DeleteDepartments(IList<Department> departments);

        /// <summary>
        /// Gets department
        /// </summary>
        /// <param name="departmentId">Department identifier</param>
        /// <returns>Department</returns>
        Department GetDepartmentById(int departmentId);

        /// <summary>
        /// Gets departments by identifier
        /// </summary>
        /// <param name="departmentIds">Department identifiers</param>
        /// <returns>Departments</returns>
        IList<Department> GetDepartmentsByIds(int[] departmentIds);

        /// <summary>
        /// Gets all departments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Departments</returns>
        IList<Department> GetAllDepartments(bool showHidden = false);

        /// <summary>
        /// Gets all departments
        /// </summary>
        /// <param name="departmentName">Department name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Departments</returns>
        IPagedList<Department> GetAllDepartments(string departmentName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<Department> SearchDepartments(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a department
        /// </summary>
        /// <param name="department">Department</param>
        void InsertDepartment(Department department);

        /// <summary>
        /// Updates the department
        /// </summary>
        /// <param name="department">Department</param>
        void UpdateDepartment(Department department);

        /// <summary>
        /// Updates the departments
        /// </summary>
        /// <param name="departments">Department</param>
        void UpdateDepartments(IList<Department> departments);

        #endregion
    }
}

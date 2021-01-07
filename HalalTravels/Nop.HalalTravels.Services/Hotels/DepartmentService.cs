using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;
using Nop.Services.Localization;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Department service
    /// </summary>
    public partial class DepartmentService : IDepartmentService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<Department> _departmentRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public DepartmentService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<Department> departmentRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _departmentRepository = departmentRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region Departments

        /// <summary>
        /// Delete a department
        /// </summary>
        /// <param name="department">Department</param>
        public virtual void DeleteDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            _departmentRepository.Delete(department);

            //event notification
            _eventPublisher.EntityDeleted(department);
        }

        /// <summary>
        /// Delete departments
        /// </summary>
        /// <param name="departments">Departments</param>
        public virtual void DeleteDepartments(IList<Department> departments)
        {
            if (departments == null)
                throw new ArgumentNullException(nameof(departments));

            _departmentRepository.Delete(departments);

            foreach (var department in departments)
            {
                //event notification
                _eventPublisher.EntityDeleted(department);
            }
        }

        /// <summary>
        /// Gets department
        /// </summary>
        /// <param name="departmentId">Department identifier</param>
        /// <returns>Department</returns>
        public virtual Department GetDepartmentById(int departmentId)
        {
            if (departmentId == 0)
                return null;

            return _departmentRepository.ToCachedGetById(departmentId);
        }

        /// <summary>
        /// Gets departments by identifier
        /// </summary>
        /// <param name="departmentIds">Department identifiers</param>
        /// <returns>Departments</returns>
        public virtual IList<Department> GetDepartmentsByIds(int[] departmentIds)
        {
            if (departmentIds == null || departmentIds.Length == 0)
                return new List<Department>();

            var query = from h in _departmentRepository.Table
                        where departmentIds.Contains(h.Id)
                        select h;

            var departments = query.ToList();

            //sort by passed identifiers
            var sortedDepartments = new List<Department>();
            foreach (var id in departmentIds)
            {
                var department = departments.FirstOrDefault(x => x.Id == id);
                if (department != null)
                    sortedDepartments.Add(department);
            }

            return sortedDepartments;
        }

        /// <summary>
        /// Inserts a department
        /// </summary>
        /// <param name="department">Department</param>
        public virtual void InsertDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            //insert
            _departmentRepository.Insert(department);

            //event notification
            _eventPublisher.EntityInserted(department);
        }

        /// <summary>
        /// Updates the department
        /// </summary>
        /// <param name="department">Department</param>
        public virtual void UpdateDepartment(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            //update
            _departmentRepository.Update(department);

            //event notification
            _eventPublisher.EntityUpdated(department);
        }

        /// <summary>
        /// Updates the departments
        /// </summary>
        /// <param name="departments">Department</param>
        public virtual void UpdateDepartments(IList<Department> departments)
        {
            if (departments == null)
                throw new ArgumentNullException(nameof(departments));

            //update
            _departmentRepository.Update(departments);

            //event notification
            foreach (var department in departments)
            {
                _eventPublisher.EntityUpdated(department);
            }
        }

        /// <summary>
        /// Gets all departments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Departments</returns>
        public virtual IList<Department> GetAllDepartments(bool showHidden = false)
        {
            var departments = GetAllDepartments(string.Empty, showHidden: showHidden).ToList();

            return departments;
        }

        /// <summary>
        /// Gets all departments
        /// </summary>
        /// <param name="departmentName">Department name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Departments</returns>
        public virtual IPagedList<Department> GetAllDepartments(string departmentName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _departmentRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(departmentName))
                query = query.Where(c => c.Name.Contains(departmentName));

            var departments = query.ToList();

            departments = departments.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Department>(departments, pageIndex, pageSize);
        }

        public virtual IPagedList<Department> SearchDepartments(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _departmentRepository.Table;

            if (!string.IsNullOrWhiteSpace(keywords))
                query = query.Where(c => c.Name.Contains(keywords));

            if (overridePublished == null)
            {
                if (!showHidden)
                    query = query.Where(c => c.IsActive);
            }
            else if (overridePublished == true)
            {
                query = query.Where(c => c.IsActive);
            }
            else
            {
                query = query.Where(c => !c.IsActive);
            }

            var departments = query.ToList();

            departments = departments.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Department>(departments, pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}

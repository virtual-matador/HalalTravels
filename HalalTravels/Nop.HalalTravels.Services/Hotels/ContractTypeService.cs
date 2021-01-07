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
    /// ContractType service
    /// </summary>
    public partial class ContractTypeService : IContractTypeService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<ContractType> _contractTypeRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public ContractTypeService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<ContractType> contractTypeRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _contractTypeRepository = contractTypeRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region ContractTypes

        /// <summary>
        /// Delete a contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        public virtual void DeleteContractType(ContractType contractType)
        {
            if (contractType == null)
                throw new ArgumentNullException(nameof(contractType));

            _contractTypeRepository.Delete(contractType);

            //event notification
            _eventPublisher.EntityDeleted(contractType);
        }

        /// <summary>
        /// Delete contractTypes
        /// </summary>
        /// <param name="contractTypes">ContractTypes</param>
        public virtual void DeleteContractTypes(IList<ContractType> contractTypes)
        {
            if (contractTypes == null)
                throw new ArgumentNullException(nameof(contractTypes));

            _contractTypeRepository.Delete(contractTypes);

            foreach (var contractType in contractTypes)
            {
                //event notification
                _eventPublisher.EntityDeleted(contractType);
            }
        }

        /// <summary>
        /// Gets contractType
        /// </summary>
        /// <param name="contractTypeId">ContractType identifier</param>
        /// <returns>ContractType</returns>
        public virtual ContractType GetContractTypeById(int contractTypeId)
        {
            if (contractTypeId == 0)
                return null;

            return _contractTypeRepository.ToCachedGetById(contractTypeId);
        }

        /// <summary>
        /// Gets contractTypes by identifier
        /// </summary>
        /// <param name="contractTypeIds">ContractType identifiers</param>
        /// <returns>ContractTypes</returns>
        public virtual IList<ContractType> GetContractTypesByIds(int[] contractTypeIds)
        {
            if (contractTypeIds == null || contractTypeIds.Length == 0)
                return new List<ContractType>();

            var query = from h in _contractTypeRepository.Table
                        where contractTypeIds.Contains(h.Id)
                        select h;

            var contractTypes = query.ToList();

            //sort by passed identifiers
            var sortedContractTypes = new List<ContractType>();
            foreach (var id in contractTypeIds)
            {
                var contractType = contractTypes.FirstOrDefault(x => x.Id == id);
                if (contractType != null)
                    sortedContractTypes.Add(contractType);
            }

            return sortedContractTypes;
        }

        /// <summary>
        /// Inserts a contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        public virtual void InsertContractType(ContractType contractType)
        {
            if (contractType == null)
                throw new ArgumentNullException(nameof(contractType));

            //insert
            _contractTypeRepository.Insert(contractType);

            //event notification
            _eventPublisher.EntityInserted(contractType);
        }

        /// <summary>
        /// Updates the contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        public virtual void UpdateContractType(ContractType contractType)
        {
            if (contractType == null)
                throw new ArgumentNullException(nameof(contractType));

            //update
            _contractTypeRepository.Update(contractType);

            //event notification
            _eventPublisher.EntityUpdated(contractType);
        }

        /// <summary>
        /// Updates the contractTypes
        /// </summary>
        /// <param name="contractTypes">ContractType</param>
        public virtual void UpdateContractTypes(IList<ContractType> contractTypes)
        {
            if (contractTypes == null)
                throw new ArgumentNullException(nameof(contractTypes));

            //update
            _contractTypeRepository.Update(contractTypes);

            //event notification
            foreach (var contractType in contractTypes)
            {
                _eventPublisher.EntityUpdated(contractType);
            }
        }

        /// <summary>
        /// Gets all contract types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ContractTypes</returns>
        public virtual IList<ContractType> GetAllContractTypes(bool showHidden = false)
        {
            var contractTypes = GetAllContractTypes(string.Empty, showHidden: showHidden).ToList();

            return contractTypes;
        }

        /// <summary>
        /// Gets all contract types
        /// </summary>
        /// <param name="contractTypeName">Contract Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ContractTypes</returns>
        public virtual IPagedList<ContractType> GetAllContractTypes(string contractTypeName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _contractTypeRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(contractTypeName))
                query = query.Where(c => c.Name.Contains(contractTypeName));

            var contractTypes = query.ToList();

            contractTypes = contractTypes.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<ContractType>(contractTypes, pageIndex, pageSize);
        }

        public virtual IPagedList<ContractType> SearchContractTypes(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _contractTypeRepository.Table;

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

            var contractTypes = query.ToList();

            contractTypes = contractTypes.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<ContractType>(contractTypes, pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}

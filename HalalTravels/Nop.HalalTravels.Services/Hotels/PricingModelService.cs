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
    /// PricingModel service
    /// </summary>
    public partial class PricingModelService : IPricingModelService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<PricingModel> _pricingModelRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public PricingModelService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<PricingModel> pricingModelRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _pricingModelRepository = pricingModelRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region PricingModels

        /// <summary>
        /// Delete a pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        public virtual void DeletePricingModel(PricingModel pricingModel)
        {
            if (pricingModel == null)
                throw new ArgumentNullException(nameof(pricingModel));

            _pricingModelRepository.Delete(pricingModel);

            //event notification
            _eventPublisher.EntityDeleted(pricingModel);
        }

        /// <summary>
        /// Delete pricingModels
        /// </summary>
        /// <param name="pricingModels">PricingModels</param>
        public virtual void DeletePricingModels(IList<PricingModel> pricingModels)
        {
            if (pricingModels == null)
                throw new ArgumentNullException(nameof(pricingModels));

            _pricingModelRepository.Delete(pricingModels);

            foreach (var pricingModel in pricingModels)
            {
                //event notification
                _eventPublisher.EntityDeleted(pricingModel);
            }
        }

        /// <summary>
        /// Gets pricingModel
        /// </summary>
        /// <param name="pricingModelId">PricingModel identifier</param>
        /// <returns>PricingModel</returns>
        public virtual PricingModel GetPricingModelById(int pricingModelId)
        {
            if (pricingModelId == 0)
                return null;

            return _pricingModelRepository.ToCachedGetById(pricingModelId);
        }

        /// <summary>
        /// Gets pricingModels by identifier
        /// </summary>
        /// <param name="pricingModelIds">PricingModel identifiers</param>
        /// <returns>PricingModels</returns>
        public virtual IList<PricingModel> GetPricingModelsByIds(int[] pricingModelIds)
        {
            if (pricingModelIds == null || pricingModelIds.Length == 0)
                return new List<PricingModel>();

            var query = from h in _pricingModelRepository.Table
                        where pricingModelIds.Contains(h.Id)
                        select h;

            var pricingModels = query.ToList();

            //sort by passed identifiers
            var sortedPricingModels = new List<PricingModel>();
            foreach (var id in pricingModelIds)
            {
                var pricingModel = pricingModels.FirstOrDefault(x => x.Id == id);
                if (pricingModel != null)
                    sortedPricingModels.Add(pricingModel);
            }

            return sortedPricingModels;
        }

        /// <summary>
        /// Inserts a pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        public virtual void InsertPricingModel(PricingModel pricingModel)
        {
            if (pricingModel == null)
                throw new ArgumentNullException(nameof(pricingModel));

            //insert
            _pricingModelRepository.Insert(pricingModel);

            //event notification
            _eventPublisher.EntityInserted(pricingModel);
        }

        /// <summary>
        /// Updates the pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        public virtual void UpdatePricingModel(PricingModel pricingModel)
        {
            if (pricingModel == null)
                throw new ArgumentNullException(nameof(pricingModel));

            //update
            _pricingModelRepository.Update(pricingModel);

            //event notification
            _eventPublisher.EntityUpdated(pricingModel);
        }

        /// <summary>
        /// Updates the pricingModels
        /// </summary>
        /// <param name="pricingModels">PricingModel</param>
        public virtual void UpdatePricingModels(IList<PricingModel> pricingModels)
        {
            if (pricingModels == null)
                throw new ArgumentNullException(nameof(pricingModels));

            //update
            _pricingModelRepository.Update(pricingModels);

            //event notification
            foreach (var pricingModel in pricingModels)
            {
                _eventPublisher.EntityUpdated(pricingModel);
            }
        }

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>PricingModels</returns>
        public virtual IList<PricingModel> GetAllPricingModels(bool showHidden = false)
        {
            var pricingModels = GetAllPricingModels(string.Empty, showHidden: showHidden).ToList();

            return pricingModels;
        }

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="pricingModelName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>PricingModels</returns>
        public virtual IPagedList<PricingModel> GetAllPricingModels(string pricingModelName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _pricingModelRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(pricingModelName))
                query = query.Where(c => c.Name.Contains(pricingModelName));

            var pricingModels = query.ToList();

            pricingModels = pricingModels.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<PricingModel>(pricingModels, pageIndex, pageSize);
        }

        public virtual IPagedList<PricingModel> SearchPricingModels(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _pricingModelRepository.Table;

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

            var pricingModels = query.ToList();

            pricingModels = pricingModels.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<PricingModel>(pricingModels, pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}

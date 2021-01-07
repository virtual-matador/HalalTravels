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
    /// Chain service
    /// </summary>
    public partial class ChainService : IChainService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<Chain> _chainRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public ChainService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<Chain> chainRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _chainRepository = chainRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region Chains

        /// <summary>
        /// Delete a chain
        /// </summary>
        /// <param name="chain">Chain</param>
        public virtual void DeleteChain(Chain chain)
        {
            if (chain == null)
                throw new ArgumentNullException(nameof(chain));

            _chainRepository.Delete(chain);

            //event notification
            _eventPublisher.EntityDeleted(chain);
        }

        /// <summary>
        /// Delete chains
        /// </summary>
        /// <param name="chains">Chains</param>
        public virtual void DeleteChains(IList<Chain> chains)
        {
            if (chains == null)
                throw new ArgumentNullException(nameof(chains));

            _chainRepository.Delete(chains);

            foreach (var chain in chains)
            {
                //event notification
                _eventPublisher.EntityDeleted(chain);
            }
        }

        /// <summary>
        /// Gets chain
        /// </summary>
        /// <param name="chainId">Chain identifier</param>
        /// <returns>Chain</returns>
        public virtual Chain GetChainById(int chainId)
        {
            if (chainId == 0)
                return null;

            return _chainRepository.ToCachedGetById(chainId);
        }

        /// <summary>
        /// Gets chains by identifier
        /// </summary>
        /// <param name="chainIds">Chain identifiers</param>
        /// <returns>Chains</returns>
        public virtual IList<Chain> GetChainsByIds(int[] chainIds)
        {
            if (chainIds == null || chainIds.Length == 0)
                return new List<Chain>();

            var query = from h in _chainRepository.Table
                where chainIds.Contains(h.Id)
                select h;

            var chains = query.ToList();

            //sort by passed identifiers
            var sortedChains = new List<Chain>();
            foreach (var id in chainIds)
            {
                var chain = chains.FirstOrDefault(x => x.Id == id);
                if (chain != null)
                    sortedChains.Add(chain);
            }

            return sortedChains;
        }

        /// <summary>
        /// Gets LL chains
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Chains</returns>
        public virtual IList<Chain> GetAllChains(bool showHidden = false)
        {
            var chains = GetAllChains(string.Empty, showHidden: showHidden).ToList();

            return chains;
        }

        /// <summary>
        /// Gets all chains
        /// </summary>
        /// <param name="chainName">Chain name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Chains</returns>
        public virtual IPagedList<Chain> GetAllChains(string chainName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _chainRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(chainName))
                query = query.Where(c => c.Name.Contains(chainName));

            var chains = query.ToList();

            chains = chains.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Chain>(chains, pageIndex, pageSize);
        }

        public virtual IPagedList<Chain> SearchChains(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _chainRepository.Table;

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

            var chains = query.ToList();

            chains = chains.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Chain>(chains, pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a chain
        /// </summary>
        /// <param name="chain">Chain</param>
        public virtual void InsertChain(Chain chain)
        {
            if (chain == null)
                throw new ArgumentNullException(nameof(chain));

            //insert
            _chainRepository.Insert(chain);

            //event notification
            _eventPublisher.EntityInserted(chain);
        }

        /// <summary>
        /// Updates the chain
        /// </summary>
        /// <param name="chain">Chain</param>
        public virtual void UpdateChain(Chain chain)
        {
            if (chain == null)
                throw new ArgumentNullException(nameof(chain));

            //update
            _chainRepository.Update(chain);

            //event notification
            _eventPublisher.EntityUpdated(chain);
        }

        /// <summary>
        /// Updates the chains
        /// </summary>
        /// <param name="chains">Chain</param>
        public virtual void UpdateChains(IList<Chain> chains)
        {
            if (chains == null)
                throw new ArgumentNullException(nameof(chains));

            //update
            _chainRepository.Update(chains);

            //event notification
            foreach (var chain in chains)
            {
                _eventPublisher.EntityUpdated(chain);
            }
        }

        #endregion

        #endregion
    }
}

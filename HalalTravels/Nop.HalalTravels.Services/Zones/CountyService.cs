using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Zones;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;
using Nop.Services.Localization;

namespace Nop.Services.Zones
{
    public partial class CountyService : ICountyService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<County> _countyRepository;
        protected readonly IRepository<Province> _provinceRepository;
        protected readonly IRepository<Country> _countryRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public CountyService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<County> countyRepository,
            IRepository<Province> provinceRepository,
            IRepository<Country> countryRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _countyRepository = countyRepository;
            _countyRepository = countyRepository;
            _provinceRepository = provinceRepository;
            _countyRepository = countyRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a county
        /// </summary>
        /// <param name="county">County</param>
        public virtual void DeleteCounty(County county)
        {
            if (county == null)
                throw new ArgumentNullException(nameof(county));

            _countyRepository.Delete(county);

            //event notification
            _eventPublisher.EntityDeleted(county);
        }

        /// <summary>
        /// Delete counties
        /// </summary>
        /// <param name="counties">Counties</param>
        public virtual void DeleteCounties(IList<County> counties)
        {
            if (counties == null)
                throw new ArgumentNullException(nameof(counties));

            _countyRepository.Delete(counties);

            foreach (var county in counties)
            {
                //event notification
                _eventPublisher.EntityDeleted(county);
            }
        }

        /// <summary>
        /// Gets county
        /// </summary>
        /// <param name="countyId">County identifier</param>
        /// <returns>County</returns>
        public virtual County GetCountyById(int countyId)
        {
            if (countyId == 0)
                return null;

            return _countyRepository.ToCachedGetById(countyId);
        }

        /// <summary>
        /// Gets counties by identifier
        /// </summary>
        /// <param name="countyIds">County identifiers</param>
        /// <returns>Counties</returns>
        public virtual IList<County> GetCountiesByIds(int[] countyIds)
        {
            if (countyIds == null || countyIds.Length == 0)
                return new List<County>();

            var query = from h in _countyRepository.Table
                where countyIds.Contains(h.Id)
                select h;

            var counties = query.ToList();

            //sort by passed identifiers
            var sortedCounties = new List<County>();
            foreach (var id in countyIds)
            {
                var county = counties.FirstOrDefault(x => x.Id == id);
                if (county != null)
                    sortedCounties.Add(county);
            }

            return sortedCounties;
        }

        /// <summary>
        /// Inserts a county
        /// </summary>
        /// <param name="county">County</param>
        public virtual void InsertCounty(County county)
        {
            if (county == null)
                throw new ArgumentNullException(nameof(county));

            //insert
            _countyRepository.Insert(county);

            //event notification
            _eventPublisher.EntityInserted(county);
        }

        /// <summary>
        /// Updates the county
        /// </summary>
        /// <param name="county">County</param>
        public virtual void UpdateCounty(County county)
        {
            if (county == null)
                throw new ArgumentNullException(nameof(county));

            //update
            _countyRepository.Update(county);

            //event notification
            _eventPublisher.EntityUpdated(county);
        }

        /// <summary>
        /// Updates the counties
        /// </summary>
        /// <param name="counties">County</param>
        public virtual void UpdateCounties(IList<County> counties)
        {
            if (counties == null)
                throw new ArgumentNullException(nameof(counties));

            //update
            _countyRepository.Update(counties);

            //event notification
            foreach (var county in counties)
            {
                _eventPublisher.EntityUpdated(county);
            }
        }

        /// <summary>
        /// Gets all counties
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Counties</returns>
        public virtual IList<County> GetAllCounties(bool showHidden = false)
        {
            var counties = GetAllCounties(string.Empty, showHidden: showHidden).ToList();

            return counties;
        }

        /// <summary>
        /// Gets all counties
        /// </summary>
        /// <param name="countyName">County name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Counties</returns>
        public virtual IPagedList<County> GetAllCounties(string countyName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _countyRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(countyName))
                query = query.Where(c => c.Name.Contains(countyName));

            var counties = query.ToList();

            counties = counties.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<County>(counties, pageIndex, pageSize);
        }

        public virtual IList<County> GetCountiesByCountryId(int countryId, bool showHidden = false)
        {
            var counties = GetCountiesByCountryId(countryId, string.Empty, showHidden: showHidden).ToList();

            return counties;
        }

        public virtual IPagedList<County> GetCountiesByCountryId(int countryId, string countyName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _countyRepository.Table;

            query = query.Join(_provinceRepository.Table.Where(p => p.CountryId == countryId), c => c.ProvinceId, p => p.Id, (c, p) => c);

            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(countyName))
                query = query.Where(c => c.Name.Contains(countyName));

            var counties = query.ToList();

            counties = counties.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<County>(counties, pageIndex, pageSize);
        }

        public virtual IList<County> GetCountiesByProvinceId(int provinceId, bool showHidden = false)
        {
            var counties = GetCountiesByProvinceId(provinceId, string.Empty, showHidden: showHidden).ToList();

            return counties;
        }

        public virtual IPagedList<County> GetCountiesByProvinceId(int provinceId, string countyName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _countyRepository.Table;

            query = query.Where(c => c.ProvinceId == provinceId);

            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(countyName))
                query = query.Where(c => c.Name.Contains(countyName));

            var counties = query.ToList();

            counties = counties.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<County>(counties, pageIndex, pageSize);
        }

        #endregion
    }
}
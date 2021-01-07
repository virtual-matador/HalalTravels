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
    public partial class ProvinceService : IProvinceService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<Province> _provinceRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public ProvinceService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<Province> provinceRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _provinceRepository = provinceRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a province
        /// </summary>
        /// <param name="province">Province</param>
        public virtual void DeleteProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            _provinceRepository.Delete(province);

            //event notification
            _eventPublisher.EntityDeleted(province);
        }

        /// <summary>
        /// Delete provinces
        /// </summary>
        /// <param name="provinces">Provinces</param>
        public virtual void DeleteProvinces(IList<Province> provinces)
        {
            if (provinces == null)
                throw new ArgumentNullException(nameof(provinces));

            _provinceRepository.Delete(provinces);

            foreach (var province in provinces)
            {
                //event notification
                _eventPublisher.EntityDeleted(province);
            }
        }

        /// <summary>
        /// Gets province
        /// </summary>
        /// <param name="provinceId">Province identifier</param>
        /// <returns>Province</returns>
        public virtual Province GetProvinceById(int provinceId)
        {
            if (provinceId == 0)
                return null;

            return _provinceRepository.ToCachedGetById(provinceId);
        }

        /// <summary>
        /// Gets provinces by identifier
        /// </summary>
        /// <param name="provinceIds">Province identifiers</param>
        /// <returns>Provinces</returns>
        public virtual IList<Province> GetProvincesByIds(int[] provinceIds)
        {
            if (provinceIds == null || provinceIds.Length == 0)
                return new List<Province>();

            var query = from h in _provinceRepository.Table
                        where provinceIds.Contains(h.Id)
                        select h;

            var provinces = query.ToList();

            //sort by passed identifiers
            var sortedProvinces = new List<Province>();
            foreach (var id in provinceIds)
            {
                var province = provinces.FirstOrDefault(x => x.Id == id);
                if (province != null)
                    sortedProvinces.Add(province);
            }

            return sortedProvinces;
        }

        /// <summary>
        /// Inserts a province
        /// </summary>
        /// <param name="province">Province</param>
        public virtual void InsertProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            //insert
            _provinceRepository.Insert(province);

            //event notification
            _eventPublisher.EntityInserted(province);
        }

        /// <summary>
        /// Updates the province
        /// </summary>
        /// <param name="province">Province</param>
        public virtual void UpdateProvince(Province province)
        {
            if (province == null)
                throw new ArgumentNullException(nameof(province));

            //update
            _provinceRepository.Update(province);

            //event notification
            _eventPublisher.EntityUpdated(province);
        }

        /// <summary>
        /// Updates the provinces
        /// </summary>
        /// <param name="provinces">Province</param>
        public virtual void UpdateProvinces(IList<Province> provinces)
        {
            if (provinces == null)
                throw new ArgumentNullException(nameof(provinces));

            //update
            _provinceRepository.Update(provinces);

            //event notification
            foreach (var province in provinces)
            {
                _eventPublisher.EntityUpdated(province);
            }
        }

        /// <summary>
        /// Gets all provinces
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Provinces</returns>
        public virtual IList<Province> GetAllProvinces(bool showHidden = false)
        {
            var provinces = GetAllProvinces(string.Empty, showHidden: showHidden).ToList();

            return provinces;
        }

        /// <summary>
        /// Gets all provinces
        /// </summary>
        /// <param name="provinceName">Province name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Provinces</returns>
        public virtual IPagedList<Province> GetAllProvinces(string provinceName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _provinceRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(provinceName))
                query = query.Where(c => c.Name.Contains(provinceName));

            var provinces = query.ToList();

            provinces = provinces.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Province>(provinces, pageIndex, pageSize);
        }

        public virtual IList<Province> GetProvincesByCountryId(int countryId, bool showHidden = false)
        {
            var provinces = GetProvincesByCountryId(countryId, string.Empty, showHidden: showHidden).ToList();

            return provinces;
        }

        public virtual IPagedList<Province> GetProvincesByCountryId(int countryId, string provinceName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _provinceRepository.Table;

            query = query.Where(p => p.CountryId == countryId);

            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(provinceName))
                query = query.Where(c => c.Name.Contains(provinceName));

            var provinces = query.ToList();

            provinces = provinces.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Province>(provinces, pageIndex, pageSize);
        }

        #endregion
    }
}

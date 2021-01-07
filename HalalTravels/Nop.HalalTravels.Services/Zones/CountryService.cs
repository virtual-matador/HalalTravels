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
    public partial class CountryService : ICountryService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<Country> _countryRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public CountryService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<Country> countryRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _countryRepository = countryRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void DeleteCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            _countryRepository.Delete(country);

            //event notification
            _eventPublisher.EntityDeleted(country);
        }

        /// <summary>
        /// Delete countries
        /// </summary>
        /// <param name="countries">Countries</param>
        public virtual void DeleteCountries(IList<Country> countries)
        {
            if (countries == null)
                throw new ArgumentNullException(nameof(countries));

            _countryRepository.Delete(countries);

            foreach (var country in countries)
            {
                //event notification
                _eventPublisher.EntityDeleted(country);
            }
        }

        /// <summary>
        /// Gets country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _countryRepository.ToCachedGetById(countryId);
        }

        /// <summary>
        /// Gets countries by identifier
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetCountriesByIds(int[] countryIds)
        {
            if (countryIds == null || countryIds.Length == 0)
                return new List<Country>();

            var query = from h in _countryRepository.Table
                        where countryIds.Contains(h.Id)
                        select h;

            var countries = query.ToList();

            //sort by passed identifiers
            var sortedCountries = new List<Country>();
            foreach (var id in countryIds)
            {
                var country = countries.FirstOrDefault(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }

            return sortedCountries;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void InsertCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            //insert
            _countryRepository.Insert(country);

            //event notification
            _eventPublisher.EntityInserted(country);
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void UpdateCountry(Country country)
        {
            if (country == null)
                throw new ArgumentNullException(nameof(country));

            //update
            _countryRepository.Update(country);

            //event notification
            _eventPublisher.EntityUpdated(country);
        }

        /// <summary>
        /// Updates the countries
        /// </summary>
        /// <param name="countries">Country</param>
        public virtual void UpdateCountries(IList<Country> countries)
        {
            if (countries == null)
                throw new ArgumentNullException(nameof(countries));

            //update
            _countryRepository.Update(countries);

            //event notification
            foreach (var country in countries)
            {
                _eventPublisher.EntityUpdated(country);
            }
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Country> GetAllCountries(bool showHidden = false)
        {
            var countries = GetAllCountries(string.Empty, showHidden: showHidden).ToList();

            return countries;
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="countryName">Country name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IPagedList<Country> GetAllCountries(string countryName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _countryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(countryName))
                query = query.Where(c => c.Name.Contains(countryName));

            var countries = query.ToList();

            countries = countries.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Country>(countries, pageIndex, pageSize);
        }

        #endregion
    }
}

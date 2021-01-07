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
    public partial class CityService : ICityService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<City> _cityRepository;
        protected readonly IRepository<County> _countyRepository;
        protected readonly IRepository<Province> _provinceRepository;
        protected readonly IRepository<Country> _countryRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public CityService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<City> cityRepository,
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
            _cityRepository = cityRepository;
            _countyRepository = countyRepository;
            _provinceRepository = provinceRepository;
            _cityRepository = cityRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a city
        /// </summary>
        /// <param name="city">City</param>
        public virtual void DeleteCity(City city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            _cityRepository.Delete(city);

            //event notification
            _eventPublisher.EntityDeleted(city);
        }

        /// <summary>
        /// Delete cities
        /// </summary>
        /// <param name="cities">Cities</param>
        public virtual void DeleteCities(IList<City> cities)
        {
            if (cities == null)
                throw new ArgumentNullException(nameof(cities));

            _cityRepository.Delete(cities);

            foreach (var city in cities)
            {
                //event notification
                _eventPublisher.EntityDeleted(city);
            }
        }

        /// <summary>
        /// Gets city
        /// </summary>
        /// <param name="cityId">City identifier</param>
        /// <returns>City</returns>
        public virtual City GetCityById(int cityId)
        {
            if (cityId == 0)
                return null;

            return _cityRepository.ToCachedGetById(cityId);
        }

        /// <summary>
        /// Gets cities by identifier
        /// </summary>
        /// <param name="cityIds">City identifiers</param>
        /// <returns>Cities</returns>
        public virtual IList<City> GetCitiesByIds(int[] cityIds)
        {
            if (cityIds == null || cityIds.Length == 0)
                return new List<City>();

            var query = from h in _cityRepository.Table
                        where cityIds.Contains(h.Id)
                        select h;

            var cities = query.ToList();

            //sort by passed identifiers
            var sortedCities = new List<City>();
            foreach (var id in cityIds)
            {
                var city = cities.FirstOrDefault(x => x.Id == id);
                if (city != null)
                    sortedCities.Add(city);
            }

            return sortedCities;
        }

        /// <summary>
        /// Inserts a city
        /// </summary>
        /// <param name="city">City</param>
        public virtual void InsertCity(City city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            //insert
            _cityRepository.Insert(city);

            //event notification
            _eventPublisher.EntityInserted(city);
        }

        /// <summary>
        /// Updates the city
        /// </summary>
        /// <param name="city">City</param>
        public virtual void UpdateCity(City city)
        {
            if (city == null)
                throw new ArgumentNullException(nameof(city));

            //update
            _cityRepository.Update(city);

            //event notification
            _eventPublisher.EntityUpdated(city);
        }

        /// <summary>
        /// Updates the cities
        /// </summary>
        /// <param name="cities">City</param>
        public virtual void UpdateCities(IList<City> cities)
        {
            if (cities == null)
                throw new ArgumentNullException(nameof(cities));

            //update
            _cityRepository.Update(cities);

            //event notification
            foreach (var city in cities)
            {
                _eventPublisher.EntityUpdated(city);
            }
        }

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cities</returns>
        public virtual IList<City> GetAllCities(bool showHidden = false)
        {
            var cities = GetAllCities(string.Empty, showHidden: showHidden).ToList();

            return cities;
        }

        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <param name="cityName">City name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cities</returns>
        public virtual IPagedList<City> GetAllCities(string cityName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _cityRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(cityName))
                query = query.Where(c => c.Name.Contains(cityName));

            var cities = query.ToList();

            cities = cities.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<City>(cities, pageIndex, pageSize);
        }

        public virtual IList<City> GetCitiesByCountryId(int countryId, bool showHidden = false)
        {
            var cities = GetCitiesByCountryId(countryId, string.Empty, showHidden: showHidden).ToList();

            return cities;
        }

        public virtual IPagedList<City> GetCitiesByCountryId(int countryId, string cityName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _cityRepository.Table;

            query = query.Join(_provinceRepository.Table.Where(p => p.CountryId == countryId), c => c.ProvinceId, p => p.Id, (c, p) => c);
            
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(cityName))
                query = query.Where(c => c.Name.Contains(cityName));

            var cities = query.ToList();

            cities = cities.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<City>(cities, pageIndex, pageSize);
        }

        public virtual IList<City> GetCitiesByProvinceId(int provinceId, bool showHidden = false)
        {
            var cities = GetCitiesByProvinceId(provinceId, string.Empty, showHidden: showHidden).ToList();

            return cities;
        }

        public virtual IPagedList<City> GetCitiesByProvinceId(int provinceId, string cityName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _cityRepository.Table;

            query = query.Where(c => c.ProvinceId == provinceId);

            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(cityName))
                query = query.Where(c => c.Name.Contains(cityName));

            var cities = query.ToList();

            cities = cities.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<City>(cities, pageIndex, pageSize);
        }

        public virtual IList<City> GetCitiesByCountyId(int countyId, bool showHidden = false)
        {
            var cities = GetCitiesByCountyId(countyId, string.Empty, showHidden: showHidden).ToList();

            return cities;
        }
        
        public virtual IPagedList<City> GetCitiesByCountyId(int countyId, string cityName, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _cityRepository.Table;

            query = query.Where(c => c.ProvinceId == countyId);

            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(cityName))
                query = query.Where(c => c.Name.Contains(cityName));

            var cities = query.ToList();

            cities = cities.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<City>(cities, pageIndex, pageSize);
        }

        #endregion
    }
}

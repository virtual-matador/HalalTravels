using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Zones;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Zones;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Hotel service
    /// </summary>
    public partial class HotelService : IHotelService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<AclRecord> _aclRepository;
        protected readonly IRepository<Hotel> _hotelRepository;
        protected readonly IRepository<HotelType> _hotelTypeRepository;
        protected readonly IRepository<Country> _countryRepository;
        protected readonly IRepository<Province> _provinceRepository;
        protected readonly IRepository<City> _cityRepository;
        protected readonly IRepository<HotelPicture> _hotelPictureRepository;
        protected readonly IRepository<HotelCategory> _hotelCategoryRepository;
        protected readonly IRepository<HotelLimitedToCountry> _hotelLimitedToCountryRepository;
        protected readonly IRepository<Picture> _pictureRepository;
        protected readonly IRepository<RelatedHotel> _relatedHotelRepository;
        protected readonly IRepository<HotelContact> _hotelContactRepository;
        protected readonly IRepository<HotelContractDocument> _hotelContractDocumentRepository;
        protected readonly IRepository<HotelCityMapping> _hotelCityMappingRepository;

        protected readonly IChainService _chainService;
        protected readonly IHotelTypeService _hotelTypeService;
        protected readonly ICountryService _countryService;
        protected readonly IProvinceService _provinceService;
        protected readonly ICityService _cityService;
        
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public HotelService(CommonSettings commonSettings, 
            ICacheKeyService cacheKeyService, 
            INopDataProvider dataProvider, 
            IEventPublisher eventPublisher, 
            ILanguageService languageService, 
            IRepository<AclRecord> aclRepository, 
            IRepository<Hotel> hotelRepository, 
            IRepository<HotelType> hotelTypeRepository, 
            IRepository<Country> countryRepository, 
            IRepository<Province> provinceRepository, 
            IRepository<City> cityRepository,
            IRepository<HotelPicture> hotelPictureRepository,
            IRepository<HotelCategory> hotelCategoryRepository,
            IRepository<HotelLimitedToCountry> hotelLimitedToCountryRepository,
            IRepository<Picture> pictureRepository,
            IRepository<RelatedHotel> relatedHotelRepository,IRepository<HotelContact> hotelContactRepository,
            IRepository<HotelContractDocument> hotelContractDocumentRepository,
            IRepository<HotelCityMapping> hotelCityMappingRepository,
            IChainService chainService, 
            IHotelTypeService hotelTypeService, 
            ICountryService countryService, 
            IProvinceService provinceService, 
            ICityService cityService, 
            IStaticCacheManager staticCacheManager, 
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _aclRepository = aclRepository;
            _hotelRepository = hotelRepository;
            _hotelTypeRepository = hotelTypeRepository;
            _countryRepository = countryRepository;
            _provinceRepository = provinceRepository;
            _cityRepository = cityRepository;
            _hotelPictureRepository = hotelPictureRepository;
            _hotelCategoryRepository = hotelCategoryRepository;
            _hotelLimitedToCountryRepository = hotelLimitedToCountryRepository;
            _relatedHotelRepository = relatedHotelRepository;
            _hotelContactRepository = hotelContactRepository;
            _hotelContractDocumentRepository = hotelContractDocumentRepository;
            _hotelCityMappingRepository = hotelCityMappingRepository;
            _chainService = chainService;
            _hotelTypeService = hotelTypeService;
            _countryService = countryService;
            _provinceService = provinceService;
            _cityService = cityService;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
            _pictureRepository = pictureRepository;
        }

        #endregion

        #region Methods

        #region Hotels

        /// <summary>
        /// Delete a hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        public virtual void DeleteHotel(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            _hotelRepository.Delete(hotel);

            //event notification
            _eventPublisher.EntityDeleted(hotel);
        }

        /// <summary>
        /// Delete hotels
        /// </summary>
        /// <param name="hotels">Hotels</param>
        public virtual void DeleteHotels(IList<Hotel> hotels)
        {
            if (hotels == null)
                throw new ArgumentNullException(nameof(hotels));

            _hotelRepository.Delete(hotels);

            foreach (var hotel in hotels)
            {
                //event notification
                _eventPublisher.EntityDeleted(hotel);
            }
        }

        /// <summary>
        /// Gets hotel
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <returns>Hotel</returns>
        public virtual Hotel GetHotelById(int hotelId)
        {
            if (hotelId == 0)
                return null;

            return _hotelRepository.ToCachedGetById(hotelId);
        }

        /// <summary>
        /// Get hotels by identifiers
        /// </summary>
        /// <param name="hotelIds">Hotel identifiers</param>
        /// <returns>Hotels</returns>
        public virtual IList<Hotel> GetHotelsByIds(int[] hotelIds)
        {
            if (hotelIds == null || hotelIds.Length == 0)
                return new List<Hotel>();

            var query = from h in _hotelRepository.Table
                        where hotelIds.Contains(h.Id)
                        select h;

            var hotels = query.ToList();

            //sort by passed identifiers
            var sortedHotels = new List<Hotel>();
            foreach (var id in hotelIds)
            {
                var hotel = hotels.FirstOrDefault(x => x.Id == id);
                if (hotel != null)
                    sortedHotels.Add(hotel);
            }

            return sortedHotels;
        }

        /// <summary>
        /// Inserts a hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        public virtual void InsertHotel(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //insert
            _hotelRepository.Insert(hotel);

            //event notification
            _eventPublisher.EntityInserted(hotel);
        }

        /// <summary>
        /// Updates the hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        public virtual void UpdateHotel(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //update
            _hotelRepository.Update(hotel);

            //event notification
            _eventPublisher.EntityUpdated(hotel);
        }

        /// <summary>
        /// Update hotels
        /// </summary>
        /// <param name="hotels">Hotels</param>
        public virtual void UpdateHotels(IList<Hotel> hotels)
        {
            if (hotels == null)
                throw new ArgumentNullException(nameof(hotels));

            //update
            _hotelRepository.Update(hotels);

            //event notification
            foreach (var hotel in hotels)
            {
                _eventPublisher.EntityUpdated(hotel);
            }
        }

        /// <summary>
        /// Get number of hotel (published and visible) in certain category
        /// </summary>
        /// <param name="hotelTypeIds">Chain identifiers</param>
        /// <param name="chainId">Chain identifiers; 0 to load all records</param>
        /// <returns>Number of hotels</returns>
        public virtual int GetNumberOfHotelsInType(IList<int> hotelTypeIds = null, int chainId = 0)
        {
            //validate "categoryIds" parameter
            if (hotelTypeIds != null && hotelTypeIds.Contains(0))
                hotelTypeIds.Remove(0);

            var query = _hotelRepository.Table;
            query = query.Where(h => h.Published);

            //category filtering
            if (hotelTypeIds != null && hotelTypeIds.Any())
            {
                query = from h in query
                        where hotelTypeIds.Contains(h.HotelTypeId)
                        select h;
            }

            //only distinct hotels
            var result = query.Select(h => h.Id).Distinct().Count();

            return result;
        }

        public virtual int GetNumberOfHotelsInCategory(IList<int> categoryIds = null)
        {
            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);

            var query = _hotelRepository.Table;
            query = query.Where(p => p.Published);

            //category filtering
            if (categoryIds != null && categoryIds.Any())
            {
                query = from p in query
                        join pc in _hotelCategoryRepository.Table on p.Id equals pc.HotelId
                        where categoryIds.Contains(pc.CategoryId)
                        select p;
            }

            //only distinct products
            var result = query.Select(p => p.Id).Distinct().Count();

            return result;
        }

        /// <summary>
        /// Search hotels
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="chainIds">Chain identifiers</param>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <param name="countryId">Country identifier; 0 to load all records</param>
        /// <param name="provinceId">Province identifier; 0 to load all records</param>
        /// <param name="countyId">County identifier; 0 to load all records</param>
        /// <param name="cityId">City identifier; 0 to load all records</param>
        /// <param name="hotelTagId">Hotel tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in hotel descriptions</param>
        /// <param name="searchHotelTags">A value indicating whether to search by a specified "keyword" in hotel tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered hotel specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" hotels
        /// false - load only "Unpublished" hotels
        /// </param>
        /// <returns>Hotels</returns>
        public virtual IPagedList<Hotel> SearchHotels(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            IList<int> chainIds = null,
            IList<int> hotelTypeIds = null,
            int countryId = 0,
            int provinceId = 0,
            int countyId = 0,
            int cityId = 0,
            int hotelTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchHotelTags = false,
            int languageId = 0,
            IList<int> filteredSpecs = null,
            HotelSortingEnum orderBy = HotelSortingEnum.Position,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            return SearchHotels(out var _, false,
                pageIndex, pageSize, categoryIds, chainIds, hotelTypeIds, countryId, provinceId, countyId, cityId, 
                hotelTagId, keywords, searchDescriptions, searchHotelTags, languageId, filteredSpecs,
                orderBy, showHidden, overridePublished);
        }

        /// <summary>
        /// Search hotels
        /// </summary>
        /// <param name="filterableSpecificationAttributeOptionIds">The specification attribute option identifiers applied to loaded hotels (all pages)</param>
        /// <param name="loadFilterableSpecificationAttributeOptionIds">A value indicating whether we should load the specification attribute option identifiers applied to loaded hotels (all pages)</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="chainIds">Chain identifiers</param>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <param name="countryId">Country identifier; 0 to load all records</param>
        /// <param name="provinceId">Province identifier; 0 to load all records</param>
        /// <param name="countyId">County identifier; 0 to load all records</param>
        /// <param name="cityId">City identifier; 0 to load all records</param>
        /// <param name="hotelTagId">Hotel tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in hotel descriptions</param>
        /// <param name="searchHotelTags">A value indicating whether to search by a specified "keyword" in hotel tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered hotel specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" hotels
        /// false - load only "Unpublished" hotels
        /// </param>
        /// <returns>Hotels</returns>
        public virtual IPagedList<Hotel> SearchHotels(
            out IList<int> filterableSpecificationAttributeOptionIds, 
            bool loadFilterableSpecificationAttributeOptionIds = false,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            IList<int> chainIds = null,
            IList<int> hotelTypeIds = null,
            int countryId = 0,
            int provinceId = 0,
            int countyId = 0,
            int cityId = 0,
            int hotelTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchHotelTags = false,
            int languageId = 0,
            IList<int> filteredSpecs = null,
            HotelSortingEnum orderBy = HotelSortingEnum.Position,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            filterableSpecificationAttributeOptionIds = new List<int>();

            //search by keyword
            var searchLocalizedValue = false;
            if (languageId > 0)
            {
                if (showHidden)
                {
                    searchLocalizedValue = true;
                }
                else
                {
                    //ensure that we have at least two published languages
                    var totalPublishedLanguages = _languageService.GetAllLanguages().Count;
                    searchLocalizedValue = totalPublishedLanguages >= 2;
                }
            }

            //validate "hotelTypeIds" parameter
            if (hotelTypeIds != null && hotelTypeIds.Contains(0))
                hotelTypeIds.Remove(0);

            //pass hotel type identifiers as comma-delimited string
            var commaSeparatedHotelTypeIds = hotelTypeIds == null ? string.Empty : string.Join(",", hotelTypeIds);

            //validate "chainIds" parameter
            if (chainIds != null && chainIds.Contains(0))
                chainIds.Remove(0);

            //pass chain identifiers as comma-delimited string
            var commaSeparatedChainIds = chainIds == null ? string.Empty : string.Join(",", chainIds);

            //pass categories identifiers as comma-delimited string
            var commaSeparatedCategoryIds = categoryIds == null ? string.Empty : string.Join(",", categoryIds);

            //pass specification identifiers as comma-delimited string
            var commaSeparatedSpecIds = string.Empty;
            if (filteredSpecs != null)
            {
                ((List<int>) filteredSpecs).Sort();
                commaSeparatedSpecIds = string.Join(",", filteredSpecs);
            }
            
            //some databases don't support int.MaxValue
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            //prepare input parameters
            var pCategoryIds = SqlParameterHelper.GetStringParameter("CategoryIds", commaSeparatedCategoryIds);
            var pHotelTypeIds = SqlParameterHelper.GetStringParameter("HotelTypeIds", commaSeparatedHotelTypeIds);
            var pChainIds = SqlParameterHelper.GetStringParameter("ChainIds", commaSeparatedChainIds);
            var pCountryId = SqlParameterHelper.GetInt32Parameter("CountryId", countryId);
            var pProvinceId = SqlParameterHelper.GetInt32Parameter("ProvinceId", provinceId);
            var pCountyId = SqlParameterHelper.GetInt32Parameter("CountyId", countyId);
            var pCityId = SqlParameterHelper.GetInt32Parameter("CityId", cityId);
            var pHotelTagId = SqlParameterHelper.GetInt32Parameter("HotelTagId", hotelTagId);
            var pKeywords = SqlParameterHelper.GetStringParameter("Keywords", keywords);
            var pSearchDescriptions = SqlParameterHelper.GetBooleanParameter("SearchDescriptions", searchDescriptions);
            var pSearchHotelTags = SqlParameterHelper.GetBooleanParameter("SearchHotelTags", searchHotelTags);
            var pUseFullTextSearch = SqlParameterHelper.GetBooleanParameter("UseFullTextSearch", _commonSettings.UseFullTextSearch);
            var pFullTextMode = SqlParameterHelper.GetInt32Parameter("FullTextMode", (int) _commonSettings.FullTextMode);
            var pFilteredSpecs = SqlParameterHelper.GetStringParameter("FilteredSpecs", commaSeparatedSpecIds);
            var pLanguageId = SqlParameterHelper.GetInt32Parameter("LanguageId", searchLocalizedValue ? languageId : 0);
            var pOrderBy = SqlParameterHelper.GetInt32Parameter("OrderBy", (int) orderBy);
            var pPageIndex = SqlParameterHelper.GetInt32Parameter("PageIndex", pageIndex);
            var pPageSize = SqlParameterHelper.GetInt32Parameter("PageSize", pageSize);
            var pShowHidden = SqlParameterHelper.GetBooleanParameter("ShowHidden", showHidden);
            var pOverridePublished = SqlParameterHelper.GetBooleanParameter("OverridePublished", overridePublished);
            var pLoadFilterableSpecificationAttributeOptionIds = SqlParameterHelper.GetBooleanParameter("LoadFilterableSpecificationAttributeOptionIds", loadFilterableSpecificationAttributeOptionIds);

            //prepare output parameters
            var pFilterableSpecificationAttributeOptionIds = SqlParameterHelper.GetOutputStringParameter("FilterableSpecificationAttributeOptionIds");
            pFilterableSpecificationAttributeOptionIds.Size = int.MaxValue - 1;
            var pTotalRecords = SqlParameterHelper.GetOutputInt32Parameter("TotalRecords");

            //invoke stored procedure
            var hotels = _hotelRepository.EntityFromSql("HotelLoadAllPaged", pCategoryIds, pHotelTypeIds, pChainIds, pCountryId, pProvinceId, pCountyId, pCityId, pHotelTagId, pKeywords, pSearchDescriptions, pSearchHotelTags, pUseFullTextSearch, pFullTextMode, pFilteredSpecs, pLanguageId, pOrderBy, pPageIndex, pPageSize, pShowHidden, pOverridePublished, pLoadFilterableSpecificationAttributeOptionIds, pFilterableSpecificationAttributeOptionIds, pTotalRecords).ToList();

            //get filterable specification attribute option identifier
            var filterableSpecificationAttributeOptionIdsStr = pFilterableSpecificationAttributeOptionIds.Value != DBNull.Value ? (string) pFilterableSpecificationAttributeOptionIds.Value : string.Empty;

            if (loadFilterableSpecificationAttributeOptionIds && !string.IsNullOrWhiteSpace(filterableSpecificationAttributeOptionIdsStr))
            {
                filterableSpecificationAttributeOptionIds = filterableSpecificationAttributeOptionIdsStr.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToList();
            }

            //return hotels
            var totalRecords = pTotalRecords.Value != DBNull.Value ? Convert.ToInt32(pTotalRecords.Value) : 0;
            
            return new PagedList<Hotel>(hotels, pageIndex, pageSize, totalRecords);
        }

        /// <summary>
        /// Gets hotels by hotel attribute
        /// </summary>
        /// <param name="hotelAttributeId">Hotel attribute identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Hotels</returns>
        public virtual IPagedList<Hotel> GetHotelsByHotelAttributeId(int hotelAttributeId,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            /*var query = from h in _hotelRepository.Table
                        join pam in _hotelAttributeMappingRepository.Table on h.Id equals pam.HotelId
                        where
                            pam.HotelAttributeId == hotelAttributeId &&
                            !h.Deleted
                        orderby h.Name
                        select h;

            return new PagedList<Hotel>(query, pageIndex, pageSize);*/

            //TODO: Discuss about hotel attributes
            return new PagedList<Hotel>(Enumerable.Empty<Hotel>().ToList(), pageIndex, pageSize);
        }

        /// <summary>
        /// Gets associated hotels
        /// </summary>
        /// <param name="chainId">Chain identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotels</returns>
        public virtual IList<Hotel> GetAssociatedHotels(int chainId, bool showHidden = false)
        {
            var query = _hotelRepository.Table;
            query = query.Where(x => x.ChainId == chainId);
            if (!showHidden)
            {
                query = query.Where(x => x.Published);
            }

            query = query.OrderBy(x => x.Id);

            var hotels = query.ToList();

            return hotels;
        }

        /// <summary>
        /// Update hotel review totals
        /// </summary>
        /// <param name="hotel">Hotel</param>
        public virtual void UpdateHotelReviewTotals(Hotel hotel)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //TODO: Discuss about reviews

            /*var approvedRatingSum = 0;
            var notApprovedRatingSum = 0;
            var approvedTotalReviews = 0;
            var notApprovedTotalReviews = 0;

            var reviews = _hotelReviewRepository.Table.Where(r => r.HotelId == hotel.Id);
            foreach (var pr in reviews)
            {
                if (pr.IsApproved)
                {
                    approvedRatingSum += pr.Rating;
                    approvedTotalReviews++;
                }
                else
                {
                    notApprovedRatingSum += pr.Rating;
                    notApprovedTotalReviews++;
                }
            }

            hotel.ApprovedRatingSum = approvedRatingSum;
            hotel.NotApprovedRatingSum = notApprovedRatingSum;
            hotel.ApprovedTotalReviews = approvedTotalReviews;
            hotel.NotApprovedTotalReviews = notApprovedTotalReviews;
            UpdateHotel(hotel);*/
        }

        /// <summary>
        /// Get a value indicating whether a hotel is available now (availability dates)
        /// </summary>
        /// <param name="hotel">Hotel</param>
        /// <param name="dateTime">Datetime to check; pass null to use current date</param>
        /// <returns>Result</returns>
        public virtual bool HotelIsAvailable(Hotel hotel, DateTime? dateTime = null)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            /*dateTime ??= DateTime.UtcNow;

            if (hotel.AvailableStartDateTimeUtc.HasValue && hotel.AvailableStartDateTimeUtc.Value > dateTime)
                return false;

            if (hotel.AvailableEndDateTimeUtc.HasValue && hotel.AvailableEndDateTimeUtc.Value < dateTime)
                return false;*/

            return true;
        }

        public virtual IList<Hotel> GetAllHotels(bool showHidden = false)
        {
            var query = _hotelRepository.Table;

            if (!showHidden)
                query = query.Where(h => h.Published);

            return query.ToList();
        }

        #endregion

        #region Hotel pictures

        /// <summary>
        /// Deletes a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        public virtual void DeleteHotelPicture(HotelPicture hotelPicture)
        {
            if (hotelPicture == null)
                throw new ArgumentNullException(nameof(hotelPicture));

            _hotelPictureRepository.Delete(hotelPicture);

            //event notification
            _eventPublisher.EntityDeleted(hotelPicture);
        }

        /// <summary>
        /// Gets a hotel pictures by hotel identifier
        /// </summary>
        /// <param name="hotelId">The hotel identifier</param>
        /// <returns>Hotel pictures</returns>
        public virtual IList<HotelPicture> GetHotelPicturesByHotelId(int hotelId)
        {
            var query = from hp in _hotelPictureRepository.Table
                        where hp.HotelId == hotelId
                        orderby hp.DisplayOrder, hp.Id
                        select hp;

            var hotelPictures = query.ToList();

            return hotelPictures;
        }

        /// <summary>
        /// Gets a hotel picture
        /// </summary>
        /// <param name="hotelPictureId">Hotel picture identifier</param>
        /// <returns>Hotel picture</returns>
        public virtual HotelPicture GetHotelPictureById(int hotelPictureId)
        {
            if (hotelPictureId == 0)
                return null;

            return _hotelPictureRepository.ToCachedGetById(hotelPictureId);
        }

        /// <summary>
        /// Inserts a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        public virtual void InsertHotelPicture(HotelPicture hotelPicture)
        {
            if (hotelPicture == null)
                throw new ArgumentNullException(nameof(hotelPicture));

            _hotelPictureRepository.Insert(hotelPicture);

            //event notification
            _eventPublisher.EntityInserted(hotelPicture);
        }

        /// <summary>
        /// Updates a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        public virtual void UpdateHotelPicture(HotelPicture hotelPicture)
        {
            if (hotelPicture == null)
                throw new ArgumentNullException(nameof(hotelPicture));

            _hotelPictureRepository.Update(hotelPicture);

            //event notification
            _eventPublisher.EntityUpdated(hotelPicture);
        }

        /// <summary>
        /// Get the IDs of all hotel images 
        /// </summary>
        /// <param name="hotelsIds">Hotels IDs</param>
        /// <returns>All picture identifiers grouped by hotel ID</returns>
        public IDictionary<int, int[]> GetHotelsImagesIds(int[] hotelsIds)
        {
            var hotelPictures = _hotelPictureRepository.Table.Where(p => hotelsIds.Contains(p.HotelId)).ToList();

            return hotelPictures.GroupBy(p => p.HotelId).ToDictionary(p => p.Key, p => p.Select(p1 => p1.PictureId).ToArray());
        }



        /// <summary>
        /// Gets pictures by hotel identifier
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        public virtual IList<Picture> GetPicturesByHotelId(int hotelId, int recordsToReturn = 0)
        {
            if (hotelId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                join pp in _hotelPictureRepository.Table on p.Id equals pp.PictureId
                orderby pp.DisplayOrder, pp.Id
                where pp.HotelId == hotelId
                select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        #endregion

        #region Related hotels

        /// <summary>
        /// Deletes a related hotel
        /// </summary>
        /// <param name="related
        /// ">Related hotel</param>
        public virtual void DeleteRelatedHotel(RelatedHotel relatedHotel)
        {
            if (relatedHotel == null)
                throw new ArgumentNullException(nameof(relatedHotel));

            _relatedHotelRepository.Delete(relatedHotel);

            //event notification
            _eventPublisher.EntityDeleted(relatedHotel);
        }

        /// <summary>
        /// Gets related hotels by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related hotels</returns>
        public virtual IList<RelatedHotel> GetRelatedHotelsByHotelId(int hotelId, bool showHidden = false)
        {
            var query = from rp in _relatedHotelRepository.Table
                join p in _hotelRepository.Table on rp.RelatedHotelId equals p.Id
                where rp.HotelId == hotelId
                      && (showHidden || p.Published)
                orderby rp.DisplayOrder
                select rp;

            var relatedHotels = query.ToList(); //.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.HotelsRelatedCacheKey, hotelId, showHidden));

            return relatedHotels;
        }

        /// <summary>
        /// Gets a related hotel
        /// </summary>
        /// <param name="relatedHotelId">Related hotel identifier</param>
        /// <returns>Related hotel</returns>
        public virtual RelatedHotel GetRelatedHotelById(int relatedHotelId)
        {
            if (relatedHotelId == 0)
                return null;

            return _relatedHotelRepository.ToCachedGetById(relatedHotelId);
        }

        /// <summary>
        /// Inserts a related hotel
        /// </summary>
        /// <param name="relatedHotel">Related hotel</param>
        public virtual void InsertRelatedHotel(RelatedHotel relatedHotel)
        {
            if (relatedHotel == null)
                throw new ArgumentNullException(nameof(relatedHotel));

            _relatedHotelRepository.Insert(relatedHotel);

            //event notification
            _eventPublisher.EntityInserted(relatedHotel);
        }

        /// <summary>
        /// Updates a related hotel
        /// </summary>
        /// <param name="relatedHotel">Related hotel</param>
        public virtual void UpdateRelatedHotel(RelatedHotel relatedHotel)
        {
            if (relatedHotel == null)
                throw new ArgumentNullException(nameof(relatedHotel));

            _relatedHotelRepository.Update(relatedHotel);

            //event notification
            _eventPublisher.EntityUpdated(relatedHotel);
        }

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId1">The first hotel identifier</param>
        /// <param name="hotelId2">The second hotel identifier</param>
        /// <returns>Related hotel</returns>
        public virtual RelatedHotel FindRelatedHotel(IList<RelatedHotel> source, int hotelId1, int hotelId2)
        {
            foreach (var relatedHotel in source)
                if (relatedHotel.HotelId == hotelId1 && relatedHotel.RelatedHotelId == hotelId2)
                    return relatedHotel;
            return null;
        }

        #endregion

        #region Limited To Country
        
        /// <summary>
        /// Deletes a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">hotelLimitedToCountry</param>
        public virtual void DeleteHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry)
        {
            if (hotelLimitedToCountry == null)
                throw new ArgumentNullException(nameof(hotelLimitedToCountry));

            _hotelLimitedToCountryRepository.Delete(hotelLimitedToCountry);

            //event notification
            _eventPublisher.EntityDeleted(hotelLimitedToCountry);
        }

        /// <summary>
        /// Gets hotelLimitedToCountries by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelLimitedToCountries</returns>
        public virtual IList<HotelLimitedToCountry> GetHotelLimitedToCountriesByHotelId(int hotelId, bool showHidden = false)
        {
            var query = from hlc in _hotelLimitedToCountryRepository.Table
                join c in _countryRepository.Table on hlc.CountryId equals c.Id
                where hlc.HotelId == hotelId
                      && (showHidden || c.IsActive)
                select hlc;

            var relatedHotels = query.ToList(); 

            return relatedHotels;
        }

        /// <summary>
        /// Gets a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountryId">Country identifier</param>
        /// <returns>HotelLimitedToCountry</returns>
        public virtual HotelLimitedToCountry GetHotelLimitedToCountryById(int hotelLimitedToCountryId)
        {
            if (hotelLimitedToCountryId == 0)
                return null;

            return _hotelLimitedToCountryRepository.ToCachedGetById(hotelLimitedToCountryId);
        }

        /// <summary>
        /// Inserts a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">HotelLimitedToCountry</param>
        public virtual void InsertHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry)
        {
            if (hotelLimitedToCountry == null)
                throw new ArgumentNullException(nameof(hotelLimitedToCountry));

            _hotelLimitedToCountryRepository.Insert(hotelLimitedToCountry);

            //event notification
            _eventPublisher.EntityInserted(hotelLimitedToCountry);
        }

        /// <summary>
        /// Updates a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">Related hotel</param>
        public virtual void UpdateHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry)
        {
            if (hotelLimitedToCountry == null)
                throw new ArgumentNullException(nameof(hotelLimitedToCountry));

            _hotelLimitedToCountryRepository.Update(hotelLimitedToCountry);

            //event notification
            _eventPublisher.EntityUpdated(hotelLimitedToCountry);
        }

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">The hotel identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>Related hotel</returns>
        public virtual HotelLimitedToCountry FindHotelLimitedToCountry(IList<HotelLimitedToCountry> source, int hotelId, int countryId)
        {
            foreach (var hotelLimitedToCountry in source)
                if (hotelLimitedToCountry.HotelId == hotelId && hotelLimitedToCountry.CountryId == countryId)
                    return hotelLimitedToCountry;
            return null;
        }
        
        #endregion
        
        #region Hotel Contacts

        /// <summary>
        /// Deletes a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        public virtual void DeleteHotelContact(HotelContact hotelContact)
        {
            if (hotelContact == null)
                throw new ArgumentNullException(nameof(hotelContact));

            _hotelContactRepository.Delete(hotelContact);

            //event notification
            _eventPublisher.EntityDeleted(hotelContact);
        }

        /// <summary>
        /// Gets hotel contacts by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotel contacts</returns>
        public virtual IList<HotelContact> GetHotelContactsByHotelId(int hotelId, bool showHidden = false)
        {
            var query = from hc in _hotelContactRepository.Table
                join h in _hotelRepository.Table on hc.HotelId equals h.Id
                where hc.HotelId == hotelId
                      && (showHidden || h.Published)
                select hc;

            var hotelContacts = query.ToList(); //.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.HotelsRelatedCacheKey, hotelId, showHidden));

            return hotelContacts;
        }

        /// <summary>
        /// Gets a hotel contact
        /// </summary>
        /// <param name="hotelContactId">Hotel contact identifier</param>
        /// <returns>Hotel contact</returns>
        public virtual HotelContact GetHotelContactById(int hotelContactId)
        {
            if (hotelContactId == 0)
                return null;

            return _hotelContactRepository.ToCachedGetById(hotelContactId);
        }

        /// <summary>
        /// Inserts a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        public virtual void InsertHotelContact(HotelContact hotelContact)
        {
            if (hotelContact == null)
                throw new ArgumentNullException(nameof(hotelContact));

            _hotelContactRepository.Insert(hotelContact);
            
            //event notification
            _eventPublisher.EntityInserted(hotelContact);

            if (!hotelContact.IsDefault)
                return;

            var contacts = GetHotelContactsByHotelId(hotelContact.HotelId);

            var defaultContacts = contacts.Where(c => c.Id != hotelContact.Id && c.IsDefault).ToList();

            foreach (var contact in defaultContacts)
            {
                contact.IsDefault = false;
                UpdateHotelContact(contact);
            }
        }

        /// <summary>
        /// Updates a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        public virtual void UpdateHotelContact(HotelContact hotelContact)
        {
            if (hotelContact == null)
                throw new ArgumentNullException(nameof(hotelContact));

            _hotelContactRepository.Update(hotelContact);

            //event notification
            _eventPublisher.EntityUpdated(hotelContact);
            
            if (!hotelContact.IsDefault)
                return;

            var contacts = GetHotelContactsByHotelId(hotelContact.HotelId);

            var defaultContacts = contacts.Where(c => c.Id != hotelContact.Id && c.IsDefault).ToList();

            foreach (var contact in defaultContacts)
            {
                contact.IsDefault = false;
                UpdateHotelContact(contact);
            }
        }

        #endregion
        
        #region Hotel contract documents

        /// <summary>
        /// Deletes a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        public virtual void DeleteHotelContractDocument(HotelContractDocument hotelContractDocument)
        {
            if (hotelContractDocument == null)
                throw new ArgumentNullException(nameof(hotelContractDocument));

            _hotelContractDocumentRepository.Delete(hotelContractDocument);

            //event notification
            _eventPublisher.EntityDeleted(hotelContractDocument);
        }

        /// <summary>
        /// Gets a hotel contract documents by hotel identifier
        /// </summary>
        /// <param name="hotelId">The hotel identifier</param>
        /// <returns>Hotel contract documents</returns>
        public virtual IList<HotelContractDocument> GetHotelContractDocumentsByHotelId(int hotelId)
        {
            var query = from hp in _hotelContractDocumentRepository.Table
                        where hp.HotelId == hotelId
                        orderby hp.DisplayOrder, hp.Id
                        select hp;

            var hotelContractDocuments = query.ToList();

            return hotelContractDocuments;
        }

        /// <summary>
        /// Gets a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocumentId">Hotel contract document identifier</param>
        /// <returns>Hotel contract document</returns>
        public virtual HotelContractDocument GetHotelContractDocumentById(int hotelContractDocumentId)
        {
            if (hotelContractDocumentId == 0)
                return null;

            return _hotelContractDocumentRepository.ToCachedGetById(hotelContractDocumentId);
        }

        /// <summary>
        /// Inserts a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        public virtual void InsertHotelContractDocument(HotelContractDocument hotelContractDocument)
        {
            if (hotelContractDocument == null)
                throw new ArgumentNullException(nameof(hotelContractDocument));

            _hotelContractDocumentRepository.Insert(hotelContractDocument);

            //event notification
            _eventPublisher.EntityInserted(hotelContractDocument);
        }

        /// <summary>
        /// Updates a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        public virtual void UpdateHotelContractDocument(HotelContractDocument hotelContractDocument)
        {
            if (hotelContractDocument == null)
                throw new ArgumentNullException(nameof(hotelContractDocument));

            _hotelContractDocumentRepository.Update(hotelContractDocument);

            //event notification
            _eventPublisher.EntityUpdated(hotelContractDocument);
        }

        /// <summary>
        /// Get the IDs of all hotel contract documents 
        /// </summary>
        /// <param name="hotelsIds">Hotels IDs</param>
        /// <returns>All contract document identifiers grouped by hotel ID</returns>
        public IDictionary<int, int[]> GetHotelsContractDocumentIds(int[] hotelsIds)
        {
            var hotelContractDocuments = _hotelContractDocumentRepository.Table.Where(p => hotelsIds.Contains(p.HotelId)).ToList();

            return hotelContractDocuments.GroupBy(p => p.HotelId).ToDictionary(p => p.Key, p => p.Select(p1 => p1.DocumentId).ToArray());
        }

        /// <summary>
        /// Gets contract documents by hotel identifier
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        public virtual IList<Picture> GetContractDocumentByHotelId(int hotelId, int recordsToReturn = 0)
        {
            if (hotelId == 0)
                return new List<Picture>();

            var query = from p in _pictureRepository.Table
                join pp in _hotelContractDocumentRepository.Table on p.Id equals pp.DocumentId
                orderby pp.DisplayOrder, pp.Id
                where pp.HotelId == hotelId
                select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        #endregion
        
        #region Hotel Cities
        
        /// <summary>
        /// Deletes a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">hotelCityMapping</param>
        public virtual void DeleteHotelCityMapping(HotelCityMapping hotelCityMapping)
        {
            if (hotelCityMapping == null)
                throw new ArgumentNullException(nameof(hotelCityMapping));

            _hotelCityMappingRepository.Delete(hotelCityMapping);

            //event notification
            _eventPublisher.EntityDeleted(hotelCityMapping);
        }

        /// <summary>
        /// Gets hotelCityMappings by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelCityMappings</returns>
        public virtual IList<HotelCityMapping> GetHotelCityMappingsByHotelId(int hotelId, bool showHidden = false)
        {
            var query = from hlc in _hotelCityMappingRepository.Table
                join c in _countryRepository.Table on hlc.CityId equals c.Id
                where hlc.HotelId == hotelId
                      && (showHidden || c.IsActive)
                select hlc;

            var relatedHotels = query.ToList(); 

            return relatedHotels;
        }

        /// <summary>
        /// Gets a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMappingId">Country identifier</param>
        /// <returns>HotelCityMapping</returns>
        public virtual HotelCityMapping GetHotelCityMappingById(int hotelCityMappingId)
        {
            if (hotelCityMappingId == 0)
                return null;

            return _hotelCityMappingRepository.ToCachedGetById(hotelCityMappingId);
        }

        /// <summary>
        /// Inserts a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">HotelCityMapping</param>
        public virtual void InsertHotelCityMapping(HotelCityMapping hotelCityMapping)
        {
            if (hotelCityMapping == null)
                throw new ArgumentNullException(nameof(hotelCityMapping));

            _hotelCityMappingRepository.Insert(hotelCityMapping);

            //event notification
            _eventPublisher.EntityInserted(hotelCityMapping);
        }

        /// <summary>
        /// Updates a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">Related hotel</param>
        public virtual void UpdateHotelCityMapping(HotelCityMapping hotelCityMapping)
        {
            if (hotelCityMapping == null)
                throw new ArgumentNullException(nameof(hotelCityMapping));

            _hotelCityMappingRepository.Update(hotelCityMapping);

            //event notification
            _eventPublisher.EntityUpdated(hotelCityMapping);
        }

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">The hotel identifier</param>
        /// <param name="cityId">The city identifier</param>
        /// <returns>Related hotel</returns>
        public virtual HotelCityMapping FindHotelCityMapping(IList<HotelCityMapping> source, int hotelId, int cityId)
        {
            foreach (var hotelCityMapping in source)
                if (hotelCityMapping.HotelId == hotelId && hotelCityMapping.CityId == cityId)
                    return hotelCityMapping;
            return null;
        }
        
        #endregion

        #endregion
    }
}

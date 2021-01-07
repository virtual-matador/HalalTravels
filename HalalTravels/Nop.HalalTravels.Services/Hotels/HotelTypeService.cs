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
    /// HotelType service
    /// </summary>
    public partial class HotelTypeService : IHotelTypeService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<HotelType> _hotelTypeRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public HotelTypeService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<HotelType> hotelTypeRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _hotelTypeRepository = hotelTypeRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region HotelTypes

        /// <summary>
        /// Delete a hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        public virtual void DeleteHotelType(HotelType hotelType)
        {
            if (hotelType == null)
                throw new ArgumentNullException(nameof(hotelType));

            _hotelTypeRepository.Delete(hotelType);

            //event notification
            _eventPublisher.EntityDeleted(hotelType);
        }

        /// <summary>
        /// Delete hotelTypes
        /// </summary>
        /// <param name="hotelTypes">HotelTypes</param>
        public virtual void DeleteHotelTypes(IList<HotelType> hotelTypes)
        {
            if (hotelTypes == null)
                throw new ArgumentNullException(nameof(hotelTypes));

            _hotelTypeRepository.Delete(hotelTypes);

            foreach (var hotelType in hotelTypes)
            {
                //event notification
                _eventPublisher.EntityDeleted(hotelType);
            }
        }

        /// <summary>
        /// Gets hotelType
        /// </summary>
        /// <param name="hotelTypeId">HotelType identifier</param>
        /// <returns>HotelType</returns>
        public virtual HotelType GetHotelTypeById(int hotelTypeId)
        {
            if (hotelTypeId == 0)
                return null;

            return _hotelTypeRepository.ToCachedGetById(hotelTypeId);
        }

        /// <summary>
        /// Gets hotelTypes by identifier
        /// </summary>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <returns>HotelTypes</returns>
        public virtual IList<HotelType> GetHotelTypesByIds(int[] hotelTypeIds)
        {
            if (hotelTypeIds == null || hotelTypeIds.Length == 0)
                return new List<HotelType>();

            var query = from h in _hotelTypeRepository.Table
                        where hotelTypeIds.Contains(h.Id)
                        select h;

            var hotelTypes = query.ToList();

            //sort by passed identifiers
            var sortedHotelTypes = new List<HotelType>();
            foreach (var id in hotelTypeIds)
            {
                var hotelType = hotelTypes.FirstOrDefault(x => x.Id == id);
                if (hotelType != null)
                    sortedHotelTypes.Add(hotelType);
            }

            return sortedHotelTypes;
        }

        /// <summary>
        /// Inserts a hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        public virtual void InsertHotelType(HotelType hotelType)
        {
            if (hotelType == null)
                throw new ArgumentNullException(nameof(hotelType));

            //insert
            _hotelTypeRepository.Insert(hotelType);

            //event notification
            _eventPublisher.EntityInserted(hotelType);
        }

        /// <summary>
        /// Updates the hotelType
        /// </summary>
        /// <param name="hotelType">HotelType</param>
        public virtual void UpdateHotelType(HotelType hotelType)
        {
            if (hotelType == null)
                throw new ArgumentNullException(nameof(hotelType));

            //update
            _hotelTypeRepository.Update(hotelType);

            //event notification
            _eventPublisher.EntityUpdated(hotelType);
        }

        /// <summary>
        /// Updates the hotelTypes
        /// </summary>
        /// <param name="hotelTypes">HotelType</param>
        public virtual void UpdateHotelTypes(IList<HotelType> hotelTypes)
        {
            if (hotelTypes == null)
                throw new ArgumentNullException(nameof(hotelTypes));

            //update
            _hotelTypeRepository.Update(hotelTypes);

            //event notification
            foreach (var hotelType in hotelTypes)
            {
                _eventPublisher.EntityUpdated(hotelType);
            }
        }

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelTypes</returns>
        public virtual IList<HotelType> GetAllHotelTypes(bool showHidden = false)
        {
            var hotelTypes = GetAllHotelTypes(string.Empty, showHidden: showHidden).ToList();

            return hotelTypes;
        }

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="hotelTypeName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelTypes</returns>
        public virtual IPagedList<HotelType> GetAllHotelTypes(string hotelTypeName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _hotelTypeRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(hotelTypeName))
                query = query.Where(c => c.Name.Contains(hotelTypeName));

            var hotelTypes = query.ToList();

            hotelTypes = hotelTypes.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<HotelType>(hotelTypes, pageIndex, pageSize);
        }

        public virtual IPagedList<HotelType> SearchHotelTypes(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _hotelTypeRepository.Table;

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

            var hotelTypes = query.ToList();

            hotelTypes = hotelTypes.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<HotelType>(hotelTypes, pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}

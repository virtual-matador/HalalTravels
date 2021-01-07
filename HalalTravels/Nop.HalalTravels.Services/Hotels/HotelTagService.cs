using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Seo;

namespace Nop.Services.Hotels
{
    public partial class HotelTagService : IHotelTagService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly ICustomerService _customerService;
        private readonly INopDataProvider _dataProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<HotelTagMapping> _hotelHotelTagMappingRepository;
        private readonly IRepository<HotelTag> _hotelTagRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public HotelTagService(CatalogSettings catalogSettings,
            ICacheKeyService cacheKeyService,
            ICustomerService customerService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            IRepository<HotelTagMapping> hotelHotelTagMappingRepository,
            IRepository<HotelTag> hotelTagRepository,
            IStaticCacheManager staticCacheManager,
            IUrlRecordService urlRecordService,
            IWorkContext workContext)
        {
            _catalogSettings = catalogSettings;
            _cacheKeyService = cacheKeyService;
            _customerService = customerService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _hotelHotelTagMappingRepository = hotelHotelTagMappingRepository;
            _hotelTagRepository = hotelTagRepository;
            _staticCacheManager = staticCacheManager;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Delete a hotel-hotel tag mapping
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        public virtual void DeleteHotelTagMapping(int hotelId, int hotelTagId)
        {
            var mappitngRecord = _hotelHotelTagMappingRepository.Table.FirstOrDefault(pptm => pptm.HotelId == hotelId && pptm.HotelTagId == hotelTagId);

            if (mappitngRecord is null)
                throw new Exception("Mppaing record not found");

            _hotelHotelTagMappingRepository.Delete(mappitngRecord);

            //event notification
            _eventPublisher.EntityDeleted(mappitngRecord);
        }

        /// <summary>
        /// Get hotel count for each of existing hotel tag
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Dictionary of "hotel tag ID : hotel count"</returns>
        private Dictionary<int, int> GetHotelCount(int storeId, bool showHidden)
        {
            var allowedCustomerRolesIds = string.Empty;
            if (!showHidden && !_catalogSettings.IgnoreAcl)
            {
                //Access control list. Allowed customer roles
                //pass customer role identifiers as comma-delimited string
                allowedCustomerRolesIds = string.Join(",", _customerService.GetCustomerRoleIds(_workContext.CurrentCustomer));
            }

            var query = from ht in _hotelHotelTagMappingRepository.Table
                group ht by ht.HotelTagId
                into htg
                select new {HotelTagId = htg.Key, HotelCount = htg.Count()};

            return query.ToDictionary(item => item.HotelTagId, item => item.HotelCount);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        public virtual void DeleteHotelTag(HotelTag hotelTag)
        {
            if (hotelTag == null)
                throw new ArgumentNullException(nameof(hotelTag));

            _hotelTagRepository.Delete(hotelTag);

            //event notification
            _eventPublisher.EntityDeleted(hotelTag);
        }

        /// <summary>
        /// Delete hotel tags
        /// </summary>
        /// <param name="hotelTags">Hotel tags</param>
        public virtual void DeleteHotelTags(IList<HotelTag> hotelTags)
        {
            if (hotelTags == null)
                throw new ArgumentNullException(nameof(hotelTags));

            foreach (var hotelTag in hotelTags)
            {
                DeleteHotelTag(hotelTag);
            }
        }

        /// <summary>
        /// Gets all hotel tags
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Hotel tags</returns>
        public virtual IList<HotelTag> GetAllHotelTags(string tagName = null)
        {
            var query = _hotelTagRepository.Table;

            var allHotelTags = query.ToList();

            if (!string.IsNullOrEmpty(tagName))
            {
                allHotelTags = allHotelTags.Where(tag => tag.Name.Contains(tagName)).ToList();
            }

            return allHotelTags;
        }

        /// <summary>
        /// Gets all hotel tags by hotel identifier
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <returns>Hotel tags</returns>
        public virtual IList<HotelTag> GetAllHotelTagsByHotelId(int hotelId)
        {
            var query = from pt in _hotelTagRepository.Table
                        join ppt in _hotelHotelTagMappingRepository.Table on pt.Id equals ppt.HotelTagId
                        where ppt.HotelId == hotelId
                        orderby pt.Id
                        select pt;

            var hotelTags = query.ToList();

            return hotelTags;
        }

        /// <summary>
        /// Gets hotel tag
        /// </summary>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <returns>Hotel tag</returns>
        public virtual HotelTag GetHotelTagById(int hotelTagId)
        {
            if (hotelTagId == 0)
                return null;

            return _hotelTagRepository.ToCachedGetById(hotelTagId);
        }

        /// <summary>
        /// Gets hotel tags
        /// </summary>
        /// <param name="hotelTagIds">Hotel tags identifiers</param>
        /// <returns>Hotel tags</returns>
        public virtual IList<HotelTag> GetHotelTagsByIds(int[] hotelTagIds)
        {
            if (hotelTagIds == null || hotelTagIds.Length == 0)
                return new List<HotelTag>();

            var query = from p in _hotelTagRepository.Table
                        where hotelTagIds.Contains(p.Id)
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Gets hotel tag by name
        /// </summary>
        /// <param name="name">Hotel tag name</param>
        /// <returns>Hotel tag</returns>
        public virtual HotelTag GetHotelTagByName(string name)
        {
            var query = from pt in _hotelTagRepository.Table
                        where pt.Name == name
                        select pt;

            var hotelTag = query.FirstOrDefault();
            return hotelTag;
        }

        /// <summary>
        /// Inserts a hotel-hotel tag mapping
        /// </summary>
        /// <param name="tagMapping">Hotel-hotel tag mapping</param>
        public virtual void InsertHotelTagMapping(HotelTagMapping tagMapping)
        {
            if (tagMapping is null)
                throw new ArgumentNullException(nameof(tagMapping));

            _hotelHotelTagMappingRepository.Insert(tagMapping);

            //event notification
            _eventPublisher.EntityInserted(tagMapping);
        }

        /// <summary>
        /// Inserts a hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        public virtual void InsertHotelTag(HotelTag hotelTag)
        {
            if (hotelTag == null)
                throw new ArgumentNullException(nameof(hotelTag));

            _hotelTagRepository.Insert(hotelTag);

            //event notification
            _eventPublisher.EntityInserted(hotelTag);
        }

        /// <summary>
        /// Indicates whether a hotel tag exists
        /// </summary>
        /// <param name="hotel">Hotel</param>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <returns>Result</returns>
        public virtual bool HotelTagExists(Hotel hotel, int hotelTagId)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            return _hotelHotelTagMappingRepository.Table.Any(pptm => pptm.HotelId == hotel.Id && pptm.HotelTagId == hotelTagId);
        }

        /// <summary>
        /// Updates the hotel tag
        /// </summary>
        /// <param name="hotelTag">Hotel tag</param>
        public virtual void UpdateHotelTag(HotelTag hotelTag)
        {
            if (hotelTag == null)
                throw new ArgumentNullException(nameof(hotelTag));

            _hotelTagRepository.Update(hotelTag);

            var seName = _urlRecordService.ValidateSeName(hotelTag, string.Empty, hotelTag.Name, true);
            _urlRecordService.SaveSlug(hotelTag, seName, 0);

            //event notification
            _eventPublisher.EntityUpdated(hotelTag);
        }

        /// <summary>
        /// Get number of hotels
        /// </summary>
        /// <param name="hotelTagId">Hotel tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Number of hotels</returns>
        public virtual int GetHotelCount(int hotelTagId, int storeId, bool showHidden = false)
        {
            var dictionary = GetHotelCount(storeId, showHidden);
            if (dictionary.ContainsKey(hotelTagId))
                return dictionary[hotelTagId];

            return 0;
        }

        /// <summary>
        /// Update hotel tags
        /// </summary>
        /// <param name="hotel">Hotel for update</param>
        /// <param name="hotelTags">Hotel tags</param>
        public virtual void UpdateHotelTags(Hotel hotel, string[] hotelTags)
        {
            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            //hotel tags
            var existingHotelTags = GetAllHotelTagsByHotelId(hotel.Id);
            var hotelTagsToRemove = new List<HotelTag>();
            foreach (var existingHotelTag in existingHotelTags)
            {
                var found = false;
                foreach (var newHotelTag in hotelTags)
                {
                    if (!existingHotelTag.Name.Equals(newHotelTag, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                {
                    hotelTagsToRemove.Add(existingHotelTag);
                }
            }

            foreach (var hotelTag in hotelTagsToRemove)
            {
                DeleteHotelTagMapping(hotel.Id, hotelTag.Id);
            }

            foreach (var hotelTagName in hotelTags)
            {
                HotelTag hotelTag;
                var hotelTag2 = GetHotelTagByName(hotelTagName);
                if (hotelTag2 == null)
                {
                    //add new hotel tag
                    hotelTag = new HotelTag
                    {
                        Name = hotelTagName
                    };
                    InsertHotelTag(hotelTag);
                }
                else
                {
                    hotelTag = hotelTag2;
                }

                if (!HotelTagExists(hotel, hotelTag.Id))
                {
                    InsertHotelTagMapping(new HotelTagMapping { HotelTagId = hotelTag.Id, HotelId = hotel.Id });
                }

                var seName = _urlRecordService.ValidateSeName(hotelTag, string.Empty, hotelTag.Name, true);
                _urlRecordService.SaveSlug(hotelTag, seName, 0);
            }
        }

        #endregion

        #region MyRegion

        protected partial class HotelTagWithCount
        {
            /// <summary>
            /// Gets or sets the entity identifier
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the hotel tag ID
            /// </summary>
            public int HotelTagId { get; set; }

            /// <summary>
            /// Gets or sets the count
            /// </summary>
            public int HotelCount { get; set; }
        }

        #endregion
    }
}

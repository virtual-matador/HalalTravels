using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;
using Nop.Services.Localization;

namespace Nop.Services.Hotels
{
    public class HotelPropertyService : IHotelPropertyService
    {
        #region Fields
        
        private readonly CommonSettings _commonSettings;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly INopDataProvider _dataProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILanguageService _languageService;
        
        private readonly IRepository<PropertyType> _propertyTypeRepository;
        private readonly IRepository<PropertyHeader> _propertyHeaderRepository;
        private readonly IRepository<PropertyDetail> _propertyDetailRepository;
        private readonly IRepository<HotelProperty> _hotelPropertyRepository;

        #endregion

        #region Ctor

        public HotelPropertyService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<PropertyType> propertyTypeRepository, 
            IRepository<PropertyHeader> propertyHeaderRepository, 
            IRepository<PropertyDetail> propertyDetailRepository, 
            IRepository<HotelProperty> hotelPropertyRepository)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            
            _propertyTypeRepository = propertyTypeRepository;
            _propertyHeaderRepository = propertyHeaderRepository;
            _propertyDetailRepository = propertyDetailRepository;
            _hotelPropertyRepository = hotelPropertyRepository;
        }

        #endregion
        
        
        #region Property types

        public virtual IList<PropertyType> GetAllPropertyTypes(bool showHidden = false)
        {
            var query = _propertyTypeRepository.Table;

            if (!showHidden)
                query = query.Where(i => i.IsActive);

            return query.ToList();
        }
        
        public virtual IList<PropertyType> GetAllPropertyTypesForHotel(bool showHidden = false)
        {
            var query = _propertyTypeRepository.Table;

            if (!showHidden)
                query = query.Where(i => i.IsActive);

            query = query.Where(i => i.Id == 1);

            return query.ToList();
        }

        #endregion
        
        #region Property headers

        public virtual IList<PropertyHeader> GetAllPropertyHeaders(bool showHidden = false)
        {
            var query = _propertyHeaderRepository.Table;

            if (!showHidden)
                query = query.Where(i => i.IsActive);

            return query.ToList();
        }

        public virtual IList<PropertyHeader> GetAllPropertyHeadersForHotel(bool showHidden = false)
        {
            var query = from propertyType in _propertyTypeRepository.Table
                join propertyHeader in _propertyHeaderRepository.Table on propertyType.Id equals propertyHeader.PropertyTypeId
                where propertyType.Id == 1
                      && (showHidden || propertyHeader.IsActive)
                select propertyHeader;

            return query.ToList();
        }

        public virtual IList<PropertyHeader> GetPropertyHeadersByPropertyTypeid(int propertyTypeId, bool showHidden = false)
        {
            var query = _propertyHeaderRepository.Table;

            if (!showHidden)
                query = query.Where(i => i.IsActive);

            query = query.Where(i => i.PropertyTypeId == propertyTypeId);

            return query.ToList();
        }

        #endregion
        
        #region Property details

        public virtual IList<PropertyDetail> GetPropertyDetailsByHeaderId(int propertyHeaderId, bool showHidden = false)
        {
            var query = _propertyDetailRepository.Table;

            if (!showHidden)
                query = query.Where(i => i.IsActive);

            query = query.Where(i => i.PropertyHeaderId == propertyHeaderId);

            return query.ToList();
        }

        #endregion
        
        #region Hotel properties

        public virtual void DeleteHotelProperty(HotelProperty hotelProperty)
        {
            if (hotelProperty == null)
                throw new ArgumentNullException(nameof(hotelProperty));

            _hotelPropertyRepository.Delete(hotelProperty);

            //event notification
            _eventPublisher.EntityDeleted(hotelProperty);
        }

        public virtual void DeleteHotelProperties(IList<HotelProperty> hotelProperties)
        {
            if (hotelProperties == null)
                throw new ArgumentNullException(nameof(hotelProperties));

            _hotelPropertyRepository.Delete(hotelProperties);

            foreach (var chain in hotelProperties)
            {
                //event notification
                _eventPublisher.EntityDeleted(chain);
            }
        }

        public virtual HotelProperty GetHotelPropertyById(int hotelPropertyId)
        {
            if (hotelPropertyId == 0)
                return null;

            return _hotelPropertyRepository.ToCachedGetById(hotelPropertyId);
        }

        public virtual IList<HotelProperty> GetHotelPropertiesByIds(int[] hotelPropertyIds)
        {
            if (hotelPropertyIds == null || hotelPropertyIds.Length == 0)
                return new List<HotelProperty>();

            var query = from h in _hotelPropertyRepository.Table
                where hotelPropertyIds.Contains(h.Id)
                select h;

            var hotelProperties = query.ToList();

            //sort by passed identifiers
            var sortedHotelProperties = new List<HotelProperty>();
            foreach (var id in hotelPropertyIds)
            {
                var hotelProperty = hotelProperties.FirstOrDefault(x => x.Id == id);
                if (hotelProperty != null)
                    sortedHotelProperties.Add(hotelProperty);
            }

            return sortedHotelProperties;
        }

        public virtual IList<HotelProperty> GetAllHotelProperties(bool showHidden = false)
        {
            var query = _hotelPropertyRepository.Table;
            
            return query.ToList();
        }

        public virtual IList<HotelProperty> GetAllHotelPropertiesByHotelId(int hotelId, bool showHidden = false)
        {
            var query = _hotelPropertyRepository.Table;

            query = query.Where(i => i.HotelId == hotelId);
            
            return query.ToList();
        }

        public virtual void InsertHotelProperty(HotelProperty hotelProperty)
        {
            if (hotelProperty == null)
                throw new ArgumentNullException(nameof(hotelProperty));

            //insert
            _hotelPropertyRepository.Insert(hotelProperty);

            //event notification
            _eventPublisher.EntityInserted(hotelProperty);
        }

        public virtual void UpdateHotelProperty(HotelProperty hotelProperty)
        {
            if (hotelProperty == null)
                throw new ArgumentNullException(nameof(hotelProperty));

            //update
            _hotelPropertyRepository.Update(hotelProperty);

            //event notification
            _eventPublisher.EntityUpdated(hotelProperty);
        }

        public virtual void UpdateHotelProperties(IList<HotelProperty> hotelProperties)
        {
            if (hotelProperties == null)
                throw new ArgumentNullException(nameof(hotelProperties));

            //update
            _hotelPropertyRepository.Update(hotelProperties);

            //event notification
            foreach (var hotelProperty in hotelProperties)
            {
                _eventPublisher.EntityUpdated(hotelProperty);
            }
        }

        #endregion
    }
}
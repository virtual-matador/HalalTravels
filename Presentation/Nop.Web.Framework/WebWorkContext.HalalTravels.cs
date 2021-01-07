using System.Linq;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Hotels;

namespace Nop.Web.Framework
{
    public partial class WebWorkContext
    {
        private Hotel _cachedDefaultHotel;
        
        public virtual Hotel DefaultHotel
        {
            get
            {
                //whether there is a cached value
                if (_cachedDefaultHotel != null)
                    return _cachedDefaultHotel;

                var defaultHotelId = _genericAttributeService.GetAttribute<int>(CurrentCustomer,
                    NopHotelDefaults.DefaultHotelIdAttribute, _storeContext.CurrentStore.Id);

                Hotel defaultHotel = null;

                if (defaultHotelId != 0)
                    defaultHotel = _hotelService.GetHotelById(defaultHotelId);

                
                _cachedDefaultHotel = defaultHotel;

                return _cachedDefaultHotel;
            }
            set
            {
                var hotelId = value?.Id ?? 0;

                _genericAttributeService.SaveAttribute(CurrentCustomer,
                    NopHotelDefaults.DefaultHotelIdAttribute, hotelId, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedDefaultHotel = null;
            }
        }
    }
}
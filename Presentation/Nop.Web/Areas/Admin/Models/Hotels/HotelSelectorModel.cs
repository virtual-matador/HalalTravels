using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public class HotelSelectorModel : BaseNopModel
    {
        #region Ctor

        public HotelSelectorModel()
        {
            AvailableHotels = new List<HotelModel>();
        }

        #endregion

        #region Properties

        public IList<HotelModel> AvailableHotels { get; set; }

        public HotelModel CurrentHotel { get; set; }

        #endregion
    }
}
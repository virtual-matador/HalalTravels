using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class AddHotelToHotelLimitedToCountryModel : BaseNopModel
    {
        #region Ctor

        public AddHotelToHotelLimitedToCountryModel()
        {
            SelectedHotelIds = new List<int>();
        }
        #endregion

        #region Properties

        public int CountryId { get; set; }

        public IList<int> SelectedHotelIds { get; set; }

        #endregion
    }
}
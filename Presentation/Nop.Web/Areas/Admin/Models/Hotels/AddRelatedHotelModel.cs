using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class AddRelatedHotelModel : BaseNopModel
    {
        #region Ctor

        public AddRelatedHotelModel()
        {
            SelectedHotelIds = new List<int>();
        }
        #endregion

        #region Properties

        public int HotelId { get; set; }

        public IList<int> SelectedHotelIds { get; set; }

        #endregion
    }
}

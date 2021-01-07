using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class AddHotelToCategoryModel : BaseNopModel
    {
        #region Ctor

        public AddHotelToCategoryModel()
        {
            SelectedHotelIds = new List<int>();
        }
        #endregion

        #region Properties

        public int CategoryId { get; set; }

        public IList<int> SelectedHotelIds { get; set; }

        #endregion
    }
}

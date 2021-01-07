using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    public partial class HotelContactSearchModel : BaseSearchModel
    {
        #region Properties
        
        public int HotelId { get; set; }

        #endregion
    }
}

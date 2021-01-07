using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Areas.Admin.Components
{
    public class DefaultHotelSelectorViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IHotelModelFactory _hotelModelFactory;

        #endregion

        #region Ctor

        public DefaultHotelSelectorViewComponent(IHotelModelFactory hotelModelFactory)
        {
            _hotelModelFactory = hotelModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            //prepare model
            var model = _hotelModelFactory.PrepareHotelSelectorModel();

            return View(model);
        }

        #endregion
    }
}
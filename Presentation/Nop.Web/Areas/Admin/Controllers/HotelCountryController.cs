using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Nop.Services.Zones;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class HotelCountryController : BaseAdminController
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly IProvinceService _provinceService;
        private readonly ICityService _cityService;
        private readonly ICountyService _countyService;

        #endregion

        #region Ctor

        public HotelCountryController(ICountryService countryService,
            IProvinceService provinceService,
            ICountyService countyService,
            ICityService cityService)
        {
            _countryService = countryService;
            _provinceService = provinceService;
            _countyService = countyService;
            _cityService = cityService;
        }

        #endregion

        public virtual IActionResult GetProvincesByCountryId(string countryId, bool? addAsterisk)
        {
            //permission validation is not required here

            // This action method gets called via an ajax request
            if (string.IsNullOrEmpty(countryId))
                throw new ArgumentNullException(nameof(countryId));

            if (!int.TryParse(countryId, out var countryIdValue))
                throw new ArgumentException("value is not an integer", nameof(countryId));


            var provinces = _provinceService.GetProvincesByCountryId(countryIdValue, true);
            var result = (from p in provinces
                select new {id = p.Id, name = p.Name}).ToList();

            if (addAsterisk.HasValue && addAsterisk.Value)
            {
                //asterisk
                result.Insert(0, new {id = 0, name = "All"});
            }

            return Json(result);
        }
        
        public virtual IActionResult GetCountiesByProvinceId(string provinceId, bool? addAsterisk)
        {
            //permission validation is not required here

            // This action method gets called via an ajax request
            if (string.IsNullOrEmpty(provinceId))
                throw new ArgumentNullException(nameof(provinceId));

            if (!int.TryParse(provinceId, out var provinceIdValue))
                throw new ArgumentException("value is not an integer", nameof(provinceId));

            var counties = _countyService.GetCountiesByProvinceId(provinceIdValue, true);
            
            var result = (from c in counties
                select new { id = c.Id, name = c.Name }).ToList();

            if (addAsterisk.HasValue && addAsterisk.Value)
            {
                //asterisk
                result.Insert(0, new { id = 0, name = "All" });
            }

            return Json(result);
        }

        public virtual IActionResult GetCitiesByCountyId(string countyId, bool? addAsterisk)
        {
            //permission validation is not required here

            // This action method gets called via an ajax request
            if (string.IsNullOrEmpty(countyId))
                throw new ArgumentNullException(nameof(countyId));

            if (!int.TryParse(countyId, out var countyIdValue))
                throw new ArgumentException("value is not an integer", nameof(countyId));


            var cities = _cityService.GetCitiesByCountyId(countyIdValue, true);
            var result = (from c in cities
                select new { id = c.Id, name = c.Name }).ToList();

            if (addAsterisk.HasValue && addAsterisk.Value)
            {
                //asterisk
                result.Insert(0, new { id = 0, name = "All" });
            }

            return Json(result);
        }
        
        public virtual IActionResult GetCitiesByProvinceId(string provinceId, bool? addAsterisk)
        {
            //permission validation is not required here

            // This action method gets called via an ajax request
            if (string.IsNullOrEmpty(provinceId))
                throw new ArgumentNullException(nameof(provinceId));

            if (!int.TryParse(provinceId, out var provinceIdValue))
                throw new ArgumentException("value is not an integer", nameof(provinceId));

            var cities = _cityService.GetCitiesByProvinceId(provinceIdValue, true);
            
            var result = (from c in cities
                select new { id = c.Id, name = c.Name }).ToList();

            if (addAsterisk.HasValue && addAsterisk.Value)
            {
                //asterisk
                result.Insert(0, new { id = 0, name = "All" });
            }

            return Json(result);
        }
    }
}

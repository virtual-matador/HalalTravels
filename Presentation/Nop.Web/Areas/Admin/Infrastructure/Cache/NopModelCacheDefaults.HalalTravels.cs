using Nop.Core.Caching;

namespace Nop.Web.Areas.Admin.Infrastructure.Cache
{
    public partial class NopModelCacheDefaults
    {
        /// <summary>
        /// Key for chains caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey ChainsListKey => new CacheKey("Nop.pres.admin.chains.list-{0}", ChainsListPrefixCacheKey);

        public static string ChainsListPrefixCacheKey => "Nop.pres.admin.chains.list";




        /// <summary>
        /// Key for hotel types caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey HotelTypesListKey => new CacheKey("Nop.pres.admin.hoteltypes.list-{0}", HotelTypesListPrefixCacheKey);

        public static string HotelTypesListPrefixCacheKey => "Nop.pres.admin.hoteltypes.list";
    }
}

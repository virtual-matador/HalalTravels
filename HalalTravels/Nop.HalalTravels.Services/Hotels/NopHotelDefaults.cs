using Nop.Core.Caching;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Represents default values related to hotel services
    /// </summary>
    public static partial class NopHotelDefaults
    {
        #region Category

        

        #endregion

        #region Hotels

        /// <summary>
        /// Gets a template of hotel name on copying
        /// </summary>
        /// <remarks>
        /// {0} : hotel name
        /// </remarks>
        public static string HotelCopyNameTemplate => "Copy of {0}";

        /// <summary>
        /// Gets default prefix for hotel
        /// </summary>
        public static string HotelAttributePrefix => "hotels_hotel_attribute_";

        #endregion

        #region Chain

        /// <summary>
        /// Gets a template of chain name on copying
        /// </summary>
        /// <remarks>
        /// {0} : chain name
        /// </remarks>
        public static string ChainCopyNameTemplate => "Copy of {0}";

        /// <summary>
        /// Gets default prefix for chain
        /// </summary>
        public static string ChainAttributePrefix => "hotels_chain_attribute_";

        #endregion

        #region HotelType

        /// <summary>
        /// Gets a template of hotelType name on copying
        /// </summary>
        /// <remarks>
        /// {0} : hotelType name
        /// </remarks>
        public static string HotelTypeCopyNameTemplate => "Copy of {0}";

        /// <summary>
        /// Gets default prefix for hotelType
        /// </summary>
        public static string HotelTypeAttributePrefix => "hotels_hotelType_attribute_";

        #endregion
    }
}

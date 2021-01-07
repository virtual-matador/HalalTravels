
namespace Nop.Core.Domain.Hotels
{
    /// <summary>
    /// Represents the hotel sorting
    /// </summary>
    public enum HotelSortingEnum
    {
        /// <summary>
        /// Position (display order)
        /// </summary>
        Position = 0,

        /// <summary>
        /// Name: A to Z
        /// </summary>
        NameAsc = 5,

        /// <summary>
        /// Name: Z to A
        /// </summary>
        NameDesc = 6,

        /// <summary>
        /// Chain: A to Z
        /// </summary>
        ChainAsc = 10,

        /// <summary>
        /// Chain: Z to A
        /// </summary>
        ChainDesc = 11,

        /// <summary>
        /// HotelType: A to Z
        /// </summary>
        HotelTypeAsc = 20,

        /// <summary>
        /// HotelType: Z to A
        /// </summary>
        HotelTypeDesc = 21
    }
}

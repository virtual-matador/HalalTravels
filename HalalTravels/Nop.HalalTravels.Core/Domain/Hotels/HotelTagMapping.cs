namespace Nop.Core.Domain.Hotels
{
    public partial class HotelTagMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the hotel identifier
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Gets or sets the hotel tag identifier
        /// </summary>
        public int HotelTagId { get; set; }
    }
}

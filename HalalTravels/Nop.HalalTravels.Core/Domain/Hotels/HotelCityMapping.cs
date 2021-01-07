namespace Nop.Core.Domain.Hotels
{
    public partial class HotelCityMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the hotel identifier
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Gets or sets the city identifier
        /// </summary>
        public int CityId { get; set; }
    }
}

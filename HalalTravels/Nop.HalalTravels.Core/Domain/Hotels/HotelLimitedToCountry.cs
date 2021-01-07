namespace Nop.Core.Domain.Hotels
{
    public partial class HotelLimitedToCountry : BaseEntity
    {
        public int HotelId { get; set; }

        public int CountryId { get; set; }
    }
}
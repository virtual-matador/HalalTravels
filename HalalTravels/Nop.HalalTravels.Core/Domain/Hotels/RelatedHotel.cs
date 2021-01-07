
namespace Nop.Core.Domain.Hotels
{
    public partial class RelatedHotel : BaseEntity
    {
        public int HotelId { get; set; }

        public int RelatedHotelId { get; set; }

        public int DisplayOrder { get; set; }
    }
}

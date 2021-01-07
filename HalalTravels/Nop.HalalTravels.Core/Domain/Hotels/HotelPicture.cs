namespace Nop.Core.Domain.Hotels
{
    public partial class HotelPicture  :BaseEntity
    {
        /// <summary>
        /// Gets or sets the hotel identifier
        /// </summary>
        public int HotelId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}

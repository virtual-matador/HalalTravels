namespace Nop.Core.Domain.Hotels
{
    public partial class HotelContractDocument : BaseEntity
    {
        #region Properies
        
        public int HotelId { get; set; }
        
        public int DocumentId { get; set; }

        public string DocumentType { get; set; }

        public int DisplayOrder { get; set; }

        #endregion
    }
}
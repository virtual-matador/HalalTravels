namespace Nop.Core.Domain.Hotels
{
    public class HotelProperty : BaseEntity
    {
        #region Properies

        public int PropertyDetailId { get; set; }
        
        public int HotelId { get; set; }

        public bool IsFree { get; set; }
        
        public string Comment { get; set; }

        #endregion
    }
}
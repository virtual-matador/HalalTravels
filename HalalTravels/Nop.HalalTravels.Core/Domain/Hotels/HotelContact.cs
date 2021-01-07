using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Hotels
{
    public partial class HotelContact : BaseEntity, ILocalizedEntity
    {
        #region Properies
        
        public int HotelId { get; set; }
        
        public int DepartmentId { get; set; }

        public string Name { get; set; }
        
        public string Surename { get; set; }
        
        public string Position { get; set; }
        
        public string Email { get; set; }
        
        public string MobileNo { get; set; }
        
        public string OfficePhones { get; set; }

        public bool IsDefault { get; set; }

        #endregion
    }
}
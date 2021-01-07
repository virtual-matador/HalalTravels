using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Hotels
{
    public class PropertyDetail : BaseEntity, ILocalizedEntity
    {
        #region Properies

        public string Name { get; set; }

        public int PropertyHeaderId { get; set; }
        
        public string OpenTravelCode { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }
}
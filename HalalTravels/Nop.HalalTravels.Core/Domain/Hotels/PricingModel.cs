using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Hotels
{
    public partial class PricingModel : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        public int DisplayOrder { get; set; }
    }
}
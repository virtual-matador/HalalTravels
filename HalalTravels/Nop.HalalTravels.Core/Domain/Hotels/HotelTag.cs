
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Hotels
{
    public partial class HotelTag : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Name { get; set; }
    }
}

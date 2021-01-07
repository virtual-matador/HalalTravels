using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Hotels
{
    public partial class Department : BaseEntity, ILocalizedEntity
    {
        #region Properies

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        #endregion
    }
}
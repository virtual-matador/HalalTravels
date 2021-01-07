﻿using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Zones
{
    public partial class Country : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        #region Properies

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        #endregion
    }
}
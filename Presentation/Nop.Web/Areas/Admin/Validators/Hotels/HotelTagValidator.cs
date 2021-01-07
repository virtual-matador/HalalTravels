using FluentValidation;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Hotels
{
    public partial class HotelTagValidator : BaseNopValidator<HotelTagModel>
    {
        public HotelTagValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hotels.HotelTags.Fields.Name.Required"));

            SetDatabaseValidationRules<ProductTag>(dataProvider);
        }
    }
}

using FluentValidation;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Hotels
{
    public partial class HotelTypeValidator : BaseNopValidator<HotelTypeModel>
    {
        public HotelTypeValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Hotels.HotelType.Fields.Title.Required"));

            SetDatabaseValidationRules<HotelType>(dataProvider);
        }
    }
}

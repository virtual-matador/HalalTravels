using FluentValidation;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Hotels
{
    public partial class HotelValidator : BaseNopValidator<HotelModel>
    {
        public HotelValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Hotels.Hotel.Fields.Name.Required"));

            RuleFor(x => x.ShortDescription)
                .MaximumLength(400);

            RuleFor(x => x.LongDescription)
                .MaximumLength(2000);

            RuleFor(x => x.AdminComment)
                .MaximumLength(2000);

            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .MaximumLength(45);

            RuleFor(x => x.AddressLine2)
                .MaximumLength(45);

            RuleFor(x => x.Tel1)
                .MaximumLength(15);

            RuleFor(x => x.Tel2)
                .MaximumLength(15);

            RuleFor(x => x.HotelTypeId)
                .NotEmpty();

            RuleFor(x => x.SeName)
                .Length(0, NopSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), NopSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Hotel>(dataProvider);
        }
    }
}

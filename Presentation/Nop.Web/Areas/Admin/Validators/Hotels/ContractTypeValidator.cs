using FluentValidation;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Hotels
{
    public partial class ContractTypeValidator : BaseNopValidator<ContractTypeModel>
    {
        public ContractTypeValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Hotels.ContractType.Fields.Title.Required"));

            SetDatabaseValidationRules<ContractType>(dataProvider);
        }
    }
}

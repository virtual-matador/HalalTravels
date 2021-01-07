using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Core.Domain.Hotels;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Hotels;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Hotels
{
    public partial class ChainValidator : BaseNopValidator<ChainModel>
    {
        public ChainValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Hotels.Chain.Fields.Name.Required"));

            SetDatabaseValidationRules<HotelType>(dataProvider);
        }
    }
}

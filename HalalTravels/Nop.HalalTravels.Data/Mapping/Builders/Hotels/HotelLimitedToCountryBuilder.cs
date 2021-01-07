using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Zones;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelLimitedToCountryBuilder : NopEntityBuilder<HotelLimitedToCountry>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelLimitedToCountry.HotelId)).AsInt32().NotNullable().ForeignKey<Hotel>()
                .WithColumn(nameof(HotelLimitedToCountry.CountryId)).AsInt32().NotNullable().ForeignKey<Country>();
        }

        #endregion
    }
}
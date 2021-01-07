using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Zones;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelCityMappingBuilder : NopEntityBuilder<HotelCityMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelCityMapping.CityId)).AsInt32().ForeignKey<City>()
                .WithColumn(nameof(HotelCityMapping.HotelId)).AsInt32().ForeignKey<Hotel>();
        }

        #endregion
    }
}

using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Zones;

namespace Nop.Data.Mapping.Builders.Zones
{
    /// <summary>
    /// Represents a city entity builder
    /// </summary>
    public partial class CityBuilder : NopEntityBuilder<City>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(City.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(City.ProvinceId)).AsInt32().NotNullable()
                .WithColumn(nameof(City.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(City.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

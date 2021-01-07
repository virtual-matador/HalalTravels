using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Zones;

namespace Nop.Data.Mapping.Builders.Zones
{
    /// <summary>
    /// Represents a country entity builder
    /// </summary>
    public partial class CountryBuilder : NopEntityBuilder<Country>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Country.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Country.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Country.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

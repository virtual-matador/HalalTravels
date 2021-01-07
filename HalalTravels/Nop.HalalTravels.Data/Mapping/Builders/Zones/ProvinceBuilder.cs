using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Zones;

namespace Nop.Data.Mapping.Builders.Zones
{
    /// <summary>
    /// Represents a province entity builder
    /// </summary>
    public partial class ProvinceBuilder : NopEntityBuilder<Province>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Province.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Province.CountryId)).AsInt32().NotNullable()
                .WithColumn(nameof(Province.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Province.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

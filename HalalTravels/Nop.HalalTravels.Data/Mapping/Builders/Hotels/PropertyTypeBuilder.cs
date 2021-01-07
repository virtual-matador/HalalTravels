using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class PropertyTypeBuilder : NopEntityBuilder<PropertyType>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PropertyType.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(PropertyType.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(PropertyType.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}
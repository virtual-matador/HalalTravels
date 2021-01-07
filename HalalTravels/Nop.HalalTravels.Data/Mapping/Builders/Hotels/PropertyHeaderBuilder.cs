using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class PropertyHeaderBuilder : NopEntityBuilder<PropertyHeader>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PropertyHeader.Name)).AsString(200).NotNullable()
                .WithColumn(nameof(PropertyHeader.PropertyTypeId)).AsInt32().ForeignKey<PropertyType>()
                .WithColumn(nameof(PropertyHeader.OpenTravelCode)).AsString(3).NotNullable()
                .WithColumn(nameof(PropertyHeader.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(PropertyHeader.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}
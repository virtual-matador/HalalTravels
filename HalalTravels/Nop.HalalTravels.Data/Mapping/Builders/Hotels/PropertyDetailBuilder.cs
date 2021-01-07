using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class PropertyDetailBuilder : NopEntityBuilder<PropertyDetail>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PropertyDetail.Name)).AsString(200).NotNullable()
                .WithColumn(nameof(PropertyDetail.PropertyHeaderId)).AsInt32().ForeignKey<PropertyHeader>()
                .WithColumn(nameof(PropertyDetail.OpenTravelCode)).AsString(3).NotNullable()
                .WithColumn(nameof(PropertyDetail.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(PropertyDetail.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}
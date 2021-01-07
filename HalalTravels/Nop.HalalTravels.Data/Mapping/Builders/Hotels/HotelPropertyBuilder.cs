using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelPropertyBuilder : NopEntityBuilder<HotelProperty>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelProperty.PropertyDetailId)).AsInt32().ForeignKey<PropertyDetail>()
                .WithColumn(nameof(HotelProperty.HotelId)).AsInt32().ForeignKey<Hotel>()
                .WithColumn(nameof(HotelProperty.IsFree)).AsBoolean().NotNullable()
                .WithColumn(nameof(HotelProperty.Comment)).AsString(400).Nullable();
        }

        #endregion
    }
}
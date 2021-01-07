using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class RelatedHotelBuilder : NopEntityBuilder<RelatedHotel>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(RelatedHotel.HotelId)).AsInt32().NotNullable()
                .WithColumn(nameof(RelatedHotel.RelatedHotelId)).AsInt32().NotNullable()
                .WithColumn(nameof(Chain.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

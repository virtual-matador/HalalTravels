using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelTagMappingBuilder : NopEntityBuilder<HotelTagMapping>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelTagMapping.HotelTagId)).AsInt32().ForeignKey<HotelTag>()
                .WithColumn(nameof(HotelTagMapping.HotelId)).AsInt32().ForeignKey<Hotel>();
        }

        #endregion
    }
}

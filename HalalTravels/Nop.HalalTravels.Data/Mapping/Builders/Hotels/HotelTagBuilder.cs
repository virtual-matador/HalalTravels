using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a hotel type entity builder
    /// </summary>
    public partial class HotelTagBuilder : NopEntityBuilder<HotelTag>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelTag.Name)).AsString(400).NotNullable();
        }

        #endregion
    }
}

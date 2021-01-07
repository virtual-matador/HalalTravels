using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelCategoryBuilder : NopEntityBuilder<HotelCategory>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelCategory.HotelId)).AsInt32().NotNullable().ForeignKey<Hotel>()
                .WithColumn(nameof(HotelCategory.CategoryId)).AsInt32().NotNullable().ForeignKey<Category>();
        }

        #endregion
    }
}

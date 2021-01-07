using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a category entity builder
    /// </summary>
    public partial class CategoryBuilder : NopEntityBuilder<Category>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Category.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Category.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Category.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

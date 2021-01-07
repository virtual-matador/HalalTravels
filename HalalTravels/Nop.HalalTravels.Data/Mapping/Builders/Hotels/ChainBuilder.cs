using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a chain entity builder
    /// </summary>
    public partial class ChainBuilder : NopEntityBuilder<Chain>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Chain.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Chain.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Chain.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

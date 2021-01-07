using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a contract type entity builder
    /// </summary>
    public partial class ContractTypeBuilder : NopEntityBuilder<ContractType>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelType.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(HotelType.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(HotelType.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

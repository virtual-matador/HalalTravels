using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public class PricingModelBuilder : NopEntityBuilder<PricingModel>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PricingModel.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(PricingModel.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(PricingModel.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}
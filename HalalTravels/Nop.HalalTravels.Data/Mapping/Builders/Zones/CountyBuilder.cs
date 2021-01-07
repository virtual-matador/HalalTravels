using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Zones;

namespace Nop.Data.Mapping.Builders.Zones
{
    public partial class CountyBuilder : NopEntityBuilder<County>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(County.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(County.ProvinceId)).AsInt32().NotNullable()
                .WithColumn(nameof(County.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(County.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}
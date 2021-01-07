using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents adepartment entity builder
    /// </summary>
    public partial class DepartmentBuilder : NopEntityBuilder<Department>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Department.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Department.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Department.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

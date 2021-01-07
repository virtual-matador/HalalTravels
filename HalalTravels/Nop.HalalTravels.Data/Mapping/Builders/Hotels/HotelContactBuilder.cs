using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a hotel contact entity builder
    /// </summary>
    public partial class HotelContactBuilder : NopEntityBuilder<HotelContact>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelContact.HotelId)).AsInt32().ForeignKey<Hotel>()
                .WithColumn(nameof(HotelContact.DepartmentId)).AsInt32().ForeignKey<Department>()
                .WithColumn(nameof(HotelContact.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(HotelContact.Surename)).AsString(400).Nullable()
                .WithColumn(nameof(HotelContact.Position)).AsString(400).Nullable()
                .WithColumn(nameof(HotelContact.Email)).AsString(200).Nullable()
                .WithColumn(nameof(HotelContact.MobileNo)).AsString(20).Nullable()
                .WithColumn(nameof(HotelContact.OfficePhones)).AsString(400).Nullable()
                .WithColumn(nameof(HotelContact.IsDefault)).AsBoolean().NotNullable();
        }

        #endregion
    }
}

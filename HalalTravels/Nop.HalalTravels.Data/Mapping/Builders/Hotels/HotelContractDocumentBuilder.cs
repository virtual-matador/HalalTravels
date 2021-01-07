using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Media;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a hotel contract document entity builder
    /// </summary>
    public partial class HotelContractDocumentBuilder : NopEntityBuilder<HotelType>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelContractDocument.HotelId)).AsInt32().ForeignKey<Hotel>()
                .WithColumn(nameof(HotelContractDocument.DocumentId)).AsInt32().ForeignKey<Picture>()
                .WithColumn(nameof(HotelContractDocument.DocumentType)).AsString(50).NotNullable()
                .WithColumn(nameof(HotelContractDocument.DisplayOrder)).AsInt32().NotNullable();
        }

        #endregion
    }
}

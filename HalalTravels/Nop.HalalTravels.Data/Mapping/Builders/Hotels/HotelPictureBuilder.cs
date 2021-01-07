using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Media;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Hotels
{
    public partial class HotelPictureBuilder : NopEntityBuilder<HotelPicture>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HotelPicture.PictureId)).AsInt32().ForeignKey<Picture>()
                .WithColumn(nameof(HotelPicture.HotelId)).AsInt32().ForeignKey<Hotel>();
        }

        #endregion
    }
}

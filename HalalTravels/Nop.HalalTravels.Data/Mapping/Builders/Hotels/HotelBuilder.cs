using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Hotels;

namespace Nop.Data.Mapping.Builders.Hotels
{
    /// <summary>
    /// Represents a hotel entity builder
    /// </summary>
    public partial class HotelBuilder : NopEntityBuilder<Hotel>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Hotel.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Hotel.MetaKeywords)).AsString(400).Nullable()
                .WithColumn(nameof(Hotel.MetaTitle)).AsString(400).Nullable()
                .WithColumn(nameof(Hotel.MetaDescription)).AsString(400).Nullable()
                .WithColumn(nameof(Hotel.HotelTypeId)).AsInt32().NotNullable() //.ForeignKey<HotelType>()
                .WithColumn(nameof(Hotel.ChainId)).AsInt32().Nullable() //.ForeignKey<Chain>()
                .WithColumn(nameof(Hotel.CountyId)).AsInt32().Nullable() 
                .WithColumn(nameof(Hotel.AddressLine1)).AsString(45).NotNullable()
                .WithColumn(nameof(Hotel.AddressLine2)).AsString(45).Nullable()
                .WithColumn(nameof(Hotel.Latitude)).AsFloat().Nullable()
                .WithColumn(nameof(Hotel.Longitude)).AsFloat().Nullable()
                .WithColumn(nameof(Hotel.PostCode)).AsString(50).Nullable()
                .WithColumn(nameof(Hotel.Tel1)).AsString(15).Nullable()
                .WithColumn(nameof(Hotel.Tel2)).AsString(15).Nullable()
                .WithColumn(nameof(Hotel.Published)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.AllowCustomerReviews)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.AvailableStartDateTimeUtc)).AsDateTime().Nullable()
                .WithColumn(nameof(Hotel.AvailableEndDateTimeUtc)).AsDateTime().Nullable()
                .WithColumn(nameof(Hotel.DisplayOrder)).AsInt32().NotNullable()
                .WithColumn(nameof(Hotel.SubjectToAcl)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.LimitedToCountries)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.DefaultCurrencyId)).AsInt32().Nullable()
                .WithColumn(nameof(Hotel.DisableWishlistButton)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.BasepriceEnabled)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.IsTaxExempt)).AsBoolean().NotNullable()
                .WithColumn(nameof(Hotel.TaxCategoryId)).AsInt32().NotNullable()
                .WithColumn(nameof(Hotel.ContractTypeId)).AsInt32().Nullable()
                .WithColumn(nameof(Hotel.PricingModelId)).AsInt32().NotNullable()
                .WithColumn(nameof(Hotel.Contractor)).AsString(400).NotNullable()
                .WithColumn(nameof(Hotel.ContractStartDateTimeUtc)).AsDateTime().Nullable()
                .WithColumn(nameof(Hotel.ContractEndDateTimeUtc)).AsDateTime().Nullable()
                .WithColumn(nameof(Hotel.AccountCode)).AsString(20).Nullable()
                .WithColumn(nameof(Hotel.AccountName)).AsString(400).Nullable()
                .WithColumn(nameof(Hotel.AccountCurrencyId)).AsInt32().Nullable()
                .WithColumn(nameof(Hotel.TaxNumber)).AsString(400).Nullable()
                .WithColumn(nameof(Hotel.CheckInTime)).AsString(5).Nullable()
                .WithColumn(nameof(Hotel.CheckOutTime)).AsString(5).Nullable();
        }

        #endregion
    }
}

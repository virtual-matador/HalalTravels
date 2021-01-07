using System;
using System.Collections.Generic;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Zones;

namespace Nop.Data.Mapping
{
    /// <summary>
    /// custom instance of backward compatibility of table naming
    /// </summary>
    public partial class HalalTravelsNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            {typeof(Hotel), "ht_hotel"},
            {typeof(Chain), "ht_chain"},
            {typeof(HotelType), "ht_hotel_type"},
            {typeof(Category), "ht_category"},
            {typeof(HotelCategory), "ht_hotel_category"},
            {typeof(HotelTag), "ht_hotel_tag"},
            {typeof(Country), "ht_country"},
            {typeof(Province), "ht_province"},
            {typeof(County), "ht_county"},
            {typeof(City), "ht_city"},
            {typeof(HotelPicture), "ht_hotel_picture_mapping"},
            {typeof(HotelTagMapping), "ht_hotel_hotel_tag_mapping"},
            {typeof(RelatedHotel), "ht_related_hotel"},
            {typeof(HotelLimitedToCountry), "ht_hotel_limited_to_country"},
            {typeof(PricingModel), "ht_pricing_model"},
            {typeof(ContractType), "ht_contract_type"},
            {typeof(Department), "ht_department"},
            {typeof(HotelContact), "ht_hotel_contact"},
            {typeof(HotelContractDocument), "ht_hotel_contract_document"},
            {typeof(HotelCityMapping), "ht_hotel_city_mapping"},
            {typeof(PropertyType), "ht_property_type"},
            {typeof(PropertyHeader), "ht_property_header"},
            {typeof(PropertyDetail), "ht_property_detail"},
            {typeof(HotelProperty), "ht_hotel_property"},
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {
            { (typeof(Country), "Name"), "Country" },
        };
    }
}

using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Media;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Hotel service
    /// </summary>
    public partial interface IHotelService
    {
        #region Hotels

        /// <summary>
        /// Delete a hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        void DeleteHotel(Hotel hotel);

        /// <summary>
        /// Delete hotels
        /// </summary>
        /// <param name="hotels">Hotels</param>
        void DeleteHotels(IList<Hotel> hotels);

        /// <summary>
        /// Gets hotel
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <returns>Hotel</returns>
        Hotel GetHotelById(int hotelId);

        /// <summary>
        /// Gets hotels by identifier
        /// </summary>
        /// <param name="hotelIds">Hotel identifiers</param>
        /// <returns>Hotels</returns>
        IList<Hotel> GetHotelsByIds(int[] hotelIds);

        /// <summary>
        /// Inserts a hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        void InsertHotel(Hotel hotel);

        /// <summary>
        /// Updates the hotel
        /// </summary>
        /// <param name="hotel">Hotel</param>
        void UpdateHotel(Hotel hotel);

        /// <summary>
        /// Updates the hotels
        /// </summary>
        /// <param name="hotels">Hotel</param>
        void UpdateHotels(IList<Hotel> hotels);

        /// <summary>
        /// Get number of hotel (published and visible) in certain category
        /// </summary>
        /// <param name="hotelTypeIds">Chain identifiers</param>
        /// <param name="chainId">Chain identifiers; 0 to load all records</param>
        /// <returns>Number of hotels</returns>
        int GetNumberOfHotelsInType(IList<int> hotelTypeIds = null, int chainId = 0);

        int GetNumberOfHotelsInCategory(IList<int> categoryIds = null);

        /// <summary>
        /// Search hotels
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="chainIds">Chain identifiers</param>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <param name="countryId">Country identifier; 0 to load all records</param>
        /// <param name="provinceId">Province identifier; 0 to load all records</param>
        /// <param name="countyId">County identifier; 0 to load all records</param>
        /// <param name="cityId">City identifier; 0 to load all records</param>
        /// <param name="hotelTagId">Hotel tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in hotel descriptions</param>
        /// <param name="searchHotelTags">A value indicating whether to search by a specified "keyword" in hotel tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered hotel specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" hotels
        /// false - load only "Unpublished" hotels
        /// </param>
        /// <returns>Hotels</returns>
        IPagedList<Hotel> SearchHotels(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            IList<int> chainIds = null,
            IList<int> hotelTypeIds = null,
            int countryId = 0,
            int provinceId = 0,
            int countyId = 0,
            int cityId = 0,
            int hotelTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchHotelTags = false,
            int languageId = 0,
            IList<int> filteredSpecs = null,
            HotelSortingEnum orderBy = HotelSortingEnum.Position,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Search hotels
        /// </summary>
        /// <param name="filterableSpecificationAttributeOptionIds">The specification attribute option identifiers applied to loaded hotels (all pages)</param>
        /// <param name="loadFilterableSpecificationAttributeOptionIds">A value indicating whether we should load the specification attribute option identifiers applied to loaded hotels (all pages)</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="chainIds">Chain identifiers</param>
        /// <param name="hotelTypeIds">HotelType identifiers</param>
        /// <param name="countryId">Country identifier; 0 to load all records</param>
        /// <param name="provinceId">Province identifier; 0 to load all records</param>
        /// <param name="countyId">County identifier; 0 to load all records</param>
        /// <param name="cityId">City identifier; 0 to load all records</param>
        /// <param name="hotelTagId">Hotel tag identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in hotel descriptions</param>
        /// <param name="searchHotelTags">A value indicating whether to search by a specified "keyword" in hotel tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered hotel specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" hotels
        /// false - load only "Unpublished" hotels
        /// </param>
        /// <returns>Hotels</returns>
        IPagedList<Hotel> SearchHotels(
            out IList<int> filterableSpecificationAttributeOptionIds,
            bool loadFilterableSpecificationAttributeOptionIds = false,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            IList<int> chainIds = null,
            IList<int> hotelTypeIds = null,
            int countryId = 0,
            int provinceId = 0,
            int countyId = 0,
            int cityId = 0,
            int hotelTagId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchHotelTags = false,
            int languageId = 0,
            IList<int> filteredSpecs = null,
            HotelSortingEnum orderBy = HotelSortingEnum.Position,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Gets hotels by hotel attribute
        /// </summary>
        /// <param name="hotelAttributeId">Hotel attribute identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Hotels</returns>
        IPagedList<Hotel> GetHotelsByHotelAttributeId(int hotelAttributeId,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets associated hotels
        /// </summary>
        /// <param name="chainId">Chain identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotels</returns>
        IList<Hotel> GetAssociatedHotels(int chainId, bool showHidden = false);

        /// <summary>
        /// Update hotel review totals
        /// </summary>
        /// <param name="hotel">Hotel</param>
        void UpdateHotelReviewTotals(Hotel hotel);

        /// <summary>
        /// Get a value indicating whether a hotel is available now (availability dates)
        /// </summary>
        /// <param name="hotel">Hotel</param>
        /// <param name="dateTime">Datetime to check; pass null to use current date</param>
        /// <returns>Result</returns>
        bool HotelIsAvailable(Hotel hotel, DateTime? dateTime = null);

        IList<Hotel> GetAllHotels(bool showHidden = false);

        #endregion

        #region Hotel pictures

        /// <summary>
        /// Deletes a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        void DeleteHotelPicture(HotelPicture hotelPicture);

        /// <summary>
        /// Gets a hotel pictures by hotel identifier
        /// </summary>
        /// <param name="hotelId">The hotel identifier</param>
        /// <returns>Hotel pictures</returns>
        IList<HotelPicture> GetHotelPicturesByHotelId(int hotelId);

        /// <summary>
        /// Gets a hotel picture
        /// </summary>
        /// <param name="hotelPictureId">Hotel picture identifier</param>
        /// <returns>Hotel picture</returns>
        HotelPicture GetHotelPictureById(int hotelPictureId);

        /// <summary>
        /// Inserts a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        void InsertHotelPicture(HotelPicture hotelPicture);

        /// <summary>
        /// Updates a hotel picture
        /// </summary>
        /// <param name="hotelPicture">Hotel picture</param>
        void UpdateHotelPicture(HotelPicture hotelPicture);

        /// <summary>
        /// Get the IDs of all hotel images 
        /// </summary>
        /// <param name="hotelsIds">Hotels IDs</param>
        /// <returns>All picture identifiers grouped by hotel ID</returns>
        IDictionary<int, int[]> GetHotelsImagesIds(int[] hotelsIds);

        /// <summary>
        /// Gets pictures by hotel identifier
        /// </summary>
        /// <param name="hotelId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Pictures</returns>
        IList<Picture> GetPicturesByHotelId(int hotelId, int recordsToReturn = 0);

        #endregion

        #region Related hotels

        /// <summary>
        /// Deletes a related hotel
        /// </summary>
        /// <param name="relatedHotel">Related hotel</param>
        void DeleteRelatedHotel(RelatedHotel relatedHotel);

        /// <summary>
        /// Gets related hotels by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related hotels</returns>
        IList<RelatedHotel> GetRelatedHotelsByHotelId(int hotelId, bool showHidden = false);

        /// <summary>
        /// Gets a related hotel
        /// </summary>
        /// <param name="relatedHotelId">Related hotel identifier</param>
        /// <returns>Related hotel</returns>
        RelatedHotel GetRelatedHotelById(int relatedHotelId);

        /// <summary>
        /// Inserts a related hotel
        /// </summary>
        /// <param name="relatedHotel">Related hotel</param>
        void InsertRelatedHotel(RelatedHotel relatedHotel);

        /// <summary>
        /// Updates a related hotel
        /// </summary>
        /// <param name="relatedHotel">Related hotel</param>
        void UpdateRelatedHotel(RelatedHotel relatedHotel);

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="relatedHotelId">The second hotel identifier</param>
        /// <returns>Related hotel</returns>
        RelatedHotel FindRelatedHotel(IList<RelatedHotel> source, int hotelId, int relatedHotelId);

        #endregion

        #region Limited To Country
        
        /// <summary>
        /// Deletes a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">hotelLimitedToCountry</param>
        void DeleteHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry);

        /// <summary>
        /// Gets hotelLimitedToCountries by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelLimitedToCountries</returns>
        IList<HotelLimitedToCountry> GetHotelLimitedToCountriesByHotelId(int hotelId, bool showHidden = false);

        /// <summary>
        /// Gets a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountryId">Country identifier</param>
        /// <returns>HotelLimitedToCountry</returns>
        HotelLimitedToCountry GetHotelLimitedToCountryById(int hotelLimitedToCountryId);

        /// <summary>
        /// Inserts a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">HotelLimitedToCountry</param>
        void InsertHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry);

        /// <summary>
        /// Updates a hotelLimitedToCountry
        /// </summary>
        /// <param name="hotelLimitedToCountry">Related hotel</param>
        void UpdateHotelLimitedToCountry(HotelLimitedToCountry hotelLimitedToCountry);

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">The hotel identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>Related hotel</returns>
        HotelLimitedToCountry FindHotelLimitedToCountry(IList<HotelLimitedToCountry> source, int hotelId, int countryId);
        
        #endregion

        #region Hotel Contacts

        /// <summary>
        /// Deletes a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        void DeleteHotelContact(HotelContact hotelContact);

        /// <summary>
        /// Gets hotel contacts by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotel contacts</returns>
        IList<HotelContact> GetHotelContactsByHotelId(int hotelId, bool showHidden = false);

        /// <summary>
        /// Gets a hotel contact
        /// </summary>
        /// <param name="hotelContactId">Hotel contact identifier</param>
        /// <returns>Hotel contact</returns>
        HotelContact GetHotelContactById(int hotelContactId);

        /// <summary>
        /// Inserts a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        void InsertHotelContact(HotelContact hotelContact);

        /// <summary>
        /// Updates a hotel contact
        /// </summary>
        /// <param name="hotelContact">Hotel contact</param>
        void UpdateHotelContact(HotelContact hotelContact);

        #endregion

        #region Contract Documents

        /// <summary>
        /// Deletes a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        void DeleteHotelContractDocument(HotelContractDocument hotelContractDocument);

        /// <summary>
        /// Gets a hotel contract documents by hotel identifier
        /// </summary>
        /// <param name="hotelId">The hotel identifier</param>
        /// <returns>Hotel contract documents</returns>
        IList<HotelContractDocument> GetHotelContractDocumentsByHotelId(int hotelId);

        /// <summary>
        /// Gets a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocumentId">Hotel contract document identifier</param>
        /// <returns>Hotel contract document</returns>
        HotelContractDocument GetHotelContractDocumentById(int hotelContractDocumentId);

        /// <summary>
        /// Inserts a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        void InsertHotelContractDocument(HotelContractDocument hotelContractDocument);

        /// <summary>
        /// Updates a hotel contract document
        /// </summary>
        /// <param name="hotelContractDocument">Hotel contract document</param>
        void UpdateHotelContractDocument(HotelContractDocument hotelContractDocument);

        /// <summary>
        /// Get the IDs of all hotel contract documents 
        /// </summary>
        /// <param name="hotelsIds">Hotels IDs</param>
        /// <returns>All contract document identifiers grouped by hotel ID</returns>
        IDictionary<int, int[]> GetHotelsContractDocumentIds(int[] hotelsIds);

        /// <summary>
        /// Gets contract documents by hotel identifier
        /// </summary>
        /// <param name="hotelId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>ContractDocuments</returns>
        IList<Picture> GetContractDocumentByHotelId(int hotelId, int recordsToReturn = 0);

        #endregion
        
        #region Hotel Cities
        
        /// <summary>
        /// Deletes a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">hotelCityMapping</param>
        void DeleteHotelCityMapping(HotelCityMapping hotelCityMapping);

        /// <summary>
        /// Gets hotelCityMappings by hotel identifier
        /// </summary>
        /// <param name="hotelId">The first hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>HotelCityMappings</returns>
        IList<HotelCityMapping> GetHotelCityMappingsByHotelId(int hotelId, bool showHidden = false);

        /// <summary>
        /// Gets a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMappingId">Country identifier</param>
        /// <returns>HotelCityMapping</returns>
        HotelCityMapping GetHotelCityMappingById(int hotelCityMappingId);

        /// <summary>
        /// Inserts a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">HotelCityMapping</param>
        void InsertHotelCityMapping(HotelCityMapping hotelCityMapping);

        /// <summary>
        /// Updates a hotelCityMapping
        /// </summary>
        /// <param name="hotelCityMapping">Related hotel</param>
        void UpdateHotelCityMapping(HotelCityMapping hotelCityMapping);

        /// <summary>
        /// Finds a related hotel item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">The hotel identifier</param>
        /// <param name="cityId">The city identifier</param>
        /// <returns>Related hotel</returns>
        HotelCityMapping FindHotelCityMapping(IList<HotelCityMapping> source, int hotelId, int cityId);
        
        #endregion
    }
}

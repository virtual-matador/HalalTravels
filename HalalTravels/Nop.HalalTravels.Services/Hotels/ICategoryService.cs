using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    public partial interface ICategoryService
    {
        #region Categories

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="category">Category</param>
        void DeleteCategory(Category category);

        /// <summary>
        /// Delete categories
        /// </summary>
        /// <param name="categories">Categories</param>
        void DeleteCategories(IList<Category> categories);

        /// <summary>
        /// Gets category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        Category GetCategoryById(int categoryId);

        /// <summary>
        /// Gets categories by identifier
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Categories</returns>
        IList<Category> GetCategoriesByIds(int[] categoryIds);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<Category> GetAllCategories(bool showHidden = false);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IPagedList<Category> GetAllCategories(string categoryName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<Category> SearchCategories(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a category
        /// </summary>
        /// <param name="category">Category</param>
        void InsertCategory(Category category);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        void UpdateCategory(Category category);

        /// <summary>
        /// Updates the categories
        /// </summary>
        /// <param name="categories">Category</param>
        void UpdateCategories(IList<Category> categories);

        #endregion

        #region HotelCategories

        /// <summary>
        /// Deletes a hotel category mapping
        /// </summary>
        /// <param name="hotelCategory">Hotel category</param>
        void DeleteHotelCategory(HotelCategory hotelCategory);

        /// <summary>
        /// Gets hotel category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotel a category mapping collection</returns>
        IPagedList<HotelCategory> GetHotelCategoriesByCategoryId(int categoryId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a hotel category mapping collection
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotel category mapping collection</returns>
        IList<HotelCategory> GetHotelCategoriesByHotelId(int hotelId, bool showHidden = false);

        /// <summary>
        /// Gets a hotel category mapping collection
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Hotel category mapping collection</returns>
        IList<HotelCategory> GetHotelCategoriesByHotelId(int hotelId, int storeId, bool showHidden = false);

        /// <summary>
        /// Gets a hotel category mapping 
        /// </summary>
        /// <param name="hotelCategoryId">Hotel category mapping identifier</param>
        /// <returns>Hotel category mapping</returns>
        HotelCategory GetHotelCategoryById(int hotelCategoryId);

        /// <summary>
        /// Inserts a hotel category mapping
        /// </summary>
        /// <param name="hotelCategory">>Hotel category mapping</param>
        void InsertHotelCategory(HotelCategory hotelCategory);

        /// <summary>
        /// Updates the hotel category mapping 
        /// </summary>
        /// <param name="hotelCategory">>Hotel category mapping</param>
        void UpdateHotelCategory(HotelCategory hotelCategory);

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="categoryIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        string[] GetNotExistingCategories(string[] categoryIdsNames);

        /// <summary>
        /// Get category IDs for hotels
        /// </summary>
        /// <param name="hotelIds">Hotels IDs</param>
        /// <returns>Category IDs for hotels</returns>
        IDictionary<int, int[]> GetHotelCategoryIds(int[] hotelIds);

        /// <summary>
        /// Returns a HotelCategory that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A HotelCategory that has the specified values; otherwise null</returns>
        HotelCategory FindHotelCategory(IList<HotelCategory> source, int hotelId, int categoryId);

        #endregion
    }
}

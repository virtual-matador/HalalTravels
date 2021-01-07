using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the hotel type model factory
    /// </summary>
    public partial interface IHotelCategoryModelFactory
    {
        /// <summary>
        /// Prepare hotel type search model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category search model</returns>
        CategorySearchModel PrepareCategorySearchModel(CategorySearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel type list model
        /// </summary>
        /// <param name="searchModel">Category search model</param>
        /// <returns>Category list model</returns>
        CategoryListModel PrepareCategoryListModel(CategorySearchModel searchModel);

        /// <summary>
        /// Prepare hotel type model
        /// </summary>
        /// <param name="model">Category model</param>
        /// <param name="chain">Category</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Category model</returns>
        CategoryModel PrepareCategoryModel(CategoryModel model, Category chain, bool excludeProperties = false);
        
        /// <summary>
        /// Prepare paged category hotel list model
        /// </summary>
        /// <param name="searchModel">Category hotel search model</param>
        /// <param name="category">Category</param>
        /// <returns>Category hotel list model</returns>
        CategoryHotelListModel PrepareCategoryHotelListModel(CategoryHotelSearchModel searchModel, Category category);

        CategoryHotelSearchModel PrepareCategoryHotelSearchModel(CategoryHotelSearchModel searchModel, Category category);

        /// <summary>
        /// Prepare hotel search model to add to the category
        /// </summary>
        /// <param name="searchModel">Hotel search model to add to the category</param>
        /// <returns>Hotel search model to add to the category</returns>
        AddHotelToCategorySearchModel PrepareAddHotelToCategorySearchModel(AddHotelToCategorySearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel list model to add to the category
        /// </summary>
        /// <param name="searchModel">Hotel search model to add to the category</param>
        /// <returns>Hotel list model to add to the category</returns>
        AddHotelToCategoryListModel PrepareAddHotelToCategoryListModel(AddHotelToCategorySearchModel searchModel);
    }
}

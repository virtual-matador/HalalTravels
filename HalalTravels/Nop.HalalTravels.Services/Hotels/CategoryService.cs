using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Hotels;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;
using Nop.Services.Localization;

namespace Nop.Services.Hotels
{
    public partial class CategoryService : ICategoryService
    {
        #region Fields

        protected readonly CommonSettings _commonSettings;
        protected readonly ICacheKeyService _cacheKeyService;
        protected readonly INopDataProvider _dataProvider;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly ILanguageService _languageService;
        protected readonly IRepository<Category> _categoryRepository;
        protected readonly IRepository<HotelCategory> _hotelCategoryRepository;
        protected readonly IRepository<Hotel> _hotelRepository;
        protected readonly IStaticCacheManager _staticCacheManager;
        protected readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public CategoryService(CommonSettings commonSettings,
            ICacheKeyService cacheKeyService,
            INopDataProvider dataProvider,
            IEventPublisher eventPublisher,
            ILanguageService languageService,
            IRepository<Category> categoryRepository,
            IRepository<HotelCategory> hotelCategoryRepository,
            IRepository<Hotel> hotelRepository,
            IStaticCacheManager staticCacheManager,
            LocalizationSettings localizationSettings)
        {
            _commonSettings = commonSettings;
            _cacheKeyService = cacheKeyService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _languageService = languageService;
            _categoryRepository = categoryRepository;
            _hotelCategoryRepository = hotelCategoryRepository;
            _hotelRepository = hotelRepository;
            _staticCacheManager = staticCacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        #region Categories

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _categoryRepository.Delete(category);

            //event notification
            _eventPublisher.EntityDeleted(category);
        }

        /// <summary>
        /// Delete categories
        /// </summary>
        /// <param name="categories">Categories</param>
        public virtual void DeleteCategories(IList<Category> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            _categoryRepository.Delete(categories);

            foreach (var category in categories)
            {
                //event notification
                _eventPublisher.EntityDeleted(category);
            }
        }

        /// <summary>
        /// Gets category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public virtual Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            return _categoryRepository.ToCachedGetById(categoryId);
        }

        /// <summary>
        /// Gets categories by identifier
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetCategoriesByIds(int[] categoryIds)
        {
            if (categoryIds == null || categoryIds.Length == 0)
                return new List<Category>();

            var query = from h in _categoryRepository.Table
                        where categoryIds.Contains(h.Id)
                        select h;

            var categories = query.ToList();

            //sort by passed identifiers
            var sortedCategories = new List<Category>();
            foreach (var id in categoryIds)
            {
                var category = categories.FirstOrDefault(x => x.Id == id);
                if (category != null)
                    sortedCategories.Add(category);
            }

            return sortedCategories;
        }

        /// <summary>
        /// Gets LL categories
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategories(bool showHidden = false)
        {
            var categories = GetAllCategories(string.Empty, showHidden: showHidden).ToList();

            return categories;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<Category> GetAllCategories(string categoryName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _categoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));

            var categories = query.ToList();

            categories = categories.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Category>(categories, pageIndex, pageSize);
        }

        public virtual IPagedList<Category> SearchCategories(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null)
        {
            var query = _categoryRepository.Table;

            if (!string.IsNullOrWhiteSpace(keywords))
                query = query.Where(c => c.Name.Contains(keywords));

            if (overridePublished == null)
            {
                if (!showHidden)
                    query = query.Where(c => c.IsActive);
            }
            else if (overridePublished == true)
            {
                query = query.Where(c => c.IsActive);
            }
            else
            {
                query = query.Where(c => !c.IsActive);
            }

            var chains = query.ToList();

            chains = chains.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList();
            //paging
            return new PagedList<Category>(chains, pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            //insert
            _categoryRepository.Insert(category);

            //event notification
            _eventPublisher.EntityInserted(category);
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            //update
            _categoryRepository.Update(category);

            //event notification
            _eventPublisher.EntityUpdated(category);
        }

        /// <summary>
        /// Updates the categories
        /// </summary>
        /// <param name="categories">Category</param>
        public virtual void UpdateCategories(IList<Category> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            //update
            _categoryRepository.Update(categories);

            //event notification
            foreach (var category in categories)
            {
                _eventPublisher.EntityUpdated(category);
            }
        }

        #endregion

        #region HotelCategories

        /// <summary>
        /// Deletes a hotel category mapping
        /// </summary>
        /// <param name="hotelCategory">Hotel category</param>
        public virtual void DeleteHotelCategory(HotelCategory hotelCategory)
        {
            if (hotelCategory == null)
                throw new ArgumentNullException(nameof(hotelCategory));

            _hotelCategoryRepository.Delete(hotelCategory);

            //event notification
            _eventPublisher.EntityDeleted(hotelCategory);
        }

        /// <summary>
        /// Gets hotel category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Hotel a category mapping collection</returns>
        public virtual IPagedList<HotelCategory> GetHotelCategoriesByCategoryId(int categoryId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<HotelCategory>(new List<HotelCategory>(), pageIndex, pageSize);

            var query = from pc in _hotelCategoryRepository.Table
                        join p in _hotelRepository.Table on pc.HotelId equals p.Id
                        where pc.CategoryId == categoryId &&
                              (showHidden || p.Published)
                        select pc;

            var hotelCategories = new PagedList<HotelCategory>(query, pageIndex, pageSize);

            return hotelCategories;
        }

        /// <summary>
        /// Gets a hotel category mapping collection
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Hotel category mapping collection</returns>
        public virtual IList<HotelCategory> GetHotelCategoriesByHotelId(int hotelId, bool showHidden = false)
        {
            return GetHotelCategoriesByHotelId(hotelId, 0, showHidden);
        }

        /// <summary>
        /// Gets a hotel category mapping collection
        /// </summary>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns>Hotel category mapping collection</returns>
        public virtual IList<HotelCategory> GetHotelCategoriesByHotelId(int hotelId, int storeId,
            bool showHidden = false)
        {
            if (hotelId == 0)
                return new List<HotelCategory>();

            var query = from pc in _hotelCategoryRepository.Table
                join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                where pc.HotelId == hotelId 
                select pc;

            if (showHidden)
                return query.ToList();

            var categoryIds = GetCategoriesByIds(query.Select(pc => pc.CategoryId).ToArray())
                .Select(c => c.Id).ToArray();

            query = from pc in query
                where categoryIds.Contains(pc.CategoryId)
                select pc;

            return query.ToList();
        }

        /// <summary>
        /// Gets a hotel category mapping 
        /// </summary>
        /// <param name="hotelCategoryId">Hotel category mapping identifier</param>
        /// <returns>Hotel category mapping</returns>
        public virtual HotelCategory GetHotelCategoryById(int hotelCategoryId)
        {
            if (hotelCategoryId == 0)
                return null;

            return _hotelCategoryRepository.ToCachedGetById(hotelCategoryId);
        }

        /// <summary>
        /// Inserts a hotel category mapping
        /// </summary>
        /// <param name="hotelCategory">>Hotel category mapping</param>
        public virtual void InsertHotelCategory(HotelCategory hotelCategory)
        {
            if (hotelCategory == null)
                throw new ArgumentNullException(nameof(hotelCategory));

            _hotelCategoryRepository.Insert(hotelCategory);

            //event notification
            _eventPublisher.EntityInserted(hotelCategory);
        }

        /// <summary>
        /// Updates the hotel category mapping 
        /// </summary>
        /// <param name="hotelCategory">>Hotel category mapping</param>
        public virtual void UpdateHotelCategory(HotelCategory hotelCategory)
        {
            if (hotelCategory == null)
                throw new ArgumentNullException(nameof(hotelCategory));

            _hotelCategoryRepository.Update(hotelCategory);

            //event notification
            _eventPublisher.EntityUpdated(hotelCategory);
        }

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="categoryIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingCategories(string[] categoryIdsNames)
        {
            if (categoryIdsNames == null)
                throw new ArgumentNullException(nameof(categoryIdsNames));

            var query = _categoryRepository.Table;
            var queryFilter = categoryIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(c => c.Name).Where(c => queryFilter.Contains(c)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = query.Select(c => c.Id.ToString()).Where(c => queryFilter.Contains(c)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        /// <summary>
        /// Get category IDs for hotels
        /// </summary>
        /// <param name="hotelIds">Hotels IDs</param>
        /// <returns>Category IDs for hotels</returns>
        public virtual IDictionary<int, int[]> GetHotelCategoryIds(int[] hotelIds)
        {
            var query = _hotelCategoryRepository.Table;

            return query.Where(p => hotelIds.Contains(p.HotelId))
                .Select(p => new { p.HotelId, p.CategoryId }).ToList()
                .GroupBy(a => a.HotelId)
                .ToDictionary(items => items.Key, items => items.Select(a => a.CategoryId).ToArray());
        }

        /// <summary>
        /// Returns a HotelCategory that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="hotelId">Hotel identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A HotelCategory that has the specified values; otherwise null</returns>
        public virtual HotelCategory FindHotelCategory(IList<HotelCategory> source, int hotelId, int categoryId)
        {
            foreach (var hotelCategory in source)
                if (hotelCategory.HotelId == hotelId && hotelCategory.CategoryId == categoryId)
                    return hotelCategory;

            return null;
        }

        #endregion

        #endregion


    }
}

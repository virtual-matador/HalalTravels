using System.Collections;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// PricingModel service
    /// </summary>
    public partial interface IPricingModelService
    {
        #region PricingModels

        /// <summary>
        /// Delete a pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        void DeletePricingModel(PricingModel pricingModel);

        /// <summary>
        /// Delete pricingModels
        /// </summary>
        /// <param name="pricingModels">PricingModels</param>
        void DeletePricingModels(IList<PricingModel> pricingModels);

        /// <summary>
        /// Gets pricingModel
        /// </summary>
        /// <param name="pricingModelId">PricingModel identifier</param>
        /// <returns>PricingModel</returns>
        PricingModel GetPricingModelById(int pricingModelId);

        /// <summary>
        /// Gets pricingModels by identifier
        /// </summary>
        /// <param name="pricingModelIds">PricingModel identifiers</param>
        /// <returns>PricingModels</returns>
        IList<PricingModel> GetPricingModelsByIds(int[] pricingModelIds);

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>PricingModels</returns>
        IList<PricingModel> GetAllPricingModels(bool showHidden = false);

        /// <summary>
        /// Gets all hotel types
        /// </summary>
        /// <param name="pricingModelName">Hotel Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>PricingModels</returns>
        IPagedList<PricingModel> GetAllPricingModels(string pricingModelName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<PricingModel> SearchPricingModels(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        void InsertPricingModel(PricingModel pricingModel);

        /// <summary>
        /// Updates the pricingModel
        /// </summary>
        /// <param name="pricingModel">PricingModel</param>
        void UpdatePricingModel(PricingModel pricingModel);

        /// <summary>
        /// Updates the pricingModels
        /// </summary>
        /// <param name="pricingModels">PricingModel</param>
        void UpdatePricingModels(IList<PricingModel> pricingModels);

        #endregion
    }
}

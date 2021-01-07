using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the pricing model model factory
    /// </summary>
    public partial interface IPricingModelModelFactory
    {
        /// <summary>
        /// Prepare pricing model search model
        /// </summary>
        /// <param name="searchModel">PricingModel search model</param>
        /// <returns>PricingModel search model</returns>
        PricingModelSearchModel PreparePricingModelSearchModel(PricingModelSearchModel searchModel);

        /// <summary>
        /// Prepare paged pricing model list model
        /// </summary>
        /// <param name="searchModel">PricingModel search model</param>
        /// <returns>PricingModel list model</returns>
        PricingModelListModel PreparePricingModelListModel(PricingModelSearchModel searchModel);

        /// <summary>
        /// Prepare pricing model model
        /// </summary>
        /// <param name="model">PricingModel model</param>
        /// <param name="pricingModel">PricingModel</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>PricingModel model</returns>
        PricingModelModel PreparePricingModelModel(PricingModelModel model, PricingModel pricingModel, bool excludeProperties = false);
    }
}

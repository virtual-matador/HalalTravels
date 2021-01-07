using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the hotel type model factory
    /// </summary>
    public partial interface IChainModelFactory
    {
        /// <summary>
        /// Prepare hotel type search model
        /// </summary>
        /// <param name="searchModel">Chain search model</param>
        /// <returns>Chain search model</returns>
        ChainSearchModel PrepareChainSearchModel(ChainSearchModel searchModel);

        /// <summary>
        /// Prepare paged hotel type list model
        /// </summary>
        /// <param name="searchModel">Chain search model</param>
        /// <returns>Chain list model</returns>
        ChainListModel PrepareChainListModel(ChainSearchModel searchModel);

        /// <summary>
        /// Prepare hotel type model
        /// </summary>
        /// <param name="model">Chain model</param>
        /// <param name="chain">Chain</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Chain model</returns>
        ChainModel PrepareChainModel(ChainModel model, Chain chain, bool excludeProperties = false);
    }
}

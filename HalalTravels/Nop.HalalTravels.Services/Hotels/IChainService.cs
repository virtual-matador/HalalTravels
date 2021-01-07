using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// Chain service
    /// </summary>
    public partial interface IChainService
    {
        #region Chains

        /// <summary>
        /// Delete a chain
        /// </summary>
        /// <param name="chain">Chain</param>
        void DeleteChain(Chain chain);

        /// <summary>
        /// Delete chains
        /// </summary>
        /// <param name="chains">Chains</param>
        void DeleteChains(IList<Chain> chains);

        /// <summary>
        /// Gets chain
        /// </summary>
        /// <param name="chainId">Chain identifier</param>
        /// <returns>Chain</returns>
        Chain GetChainById(int chainId);

        /// <summary>
        /// Gets chains by identifier
        /// </summary>
        /// <param name="chainIds">Chain identifiers</param>
        /// <returns>Chains</returns>
        IList<Chain> GetChainsByIds(int[] chainIds);

        /// <summary>
        /// Gets all chains
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Chains</returns>
        IList<Chain> GetAllChains(bool showHidden = false);

        /// <summary>
        /// Gets all chains
        /// </summary>
        /// <param name="chainName">Chain name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Chains</returns>
        IPagedList<Chain> GetAllChains(string chainName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<Chain> SearchChains(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a chain
        /// </summary>
        /// <param name="chain">Chain</param>
        void InsertChain(Chain chain);

        /// <summary>
        /// Updates the chain
        /// </summary>
        /// <param name="chain">Chain</param>
        void UpdateChain(Chain chain);

        /// <summary>
        /// Updates the chains
        /// </summary>
        /// <param name="chains">Chain</param>
        void UpdateChains(IList<Chain> chains);

        #endregion
    }
}

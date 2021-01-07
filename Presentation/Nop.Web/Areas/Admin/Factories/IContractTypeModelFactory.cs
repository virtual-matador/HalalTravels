using Nop.Core.Domain.Hotels;
using Nop.Web.Areas.Admin.Models.Hotels;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the contract type model factory
    /// </summary>
    public partial interface IContractTypeModelFactory
    {
        /// <summary>
        /// Prepare contract type search model
        /// </summary>
        /// <param name="searchModel">ContractType search model</param>
        /// <returns>ContractType search model</returns>
        ContractTypeSearchModel PrepareContractTypeSearchModel(ContractTypeSearchModel searchModel);

        /// <summary>
        /// Prepare paged contract type list model
        /// </summary>
        /// <param name="searchModel">ContractType search model</param>
        /// <returns>ContractType list model</returns>
        ContractTypeListModel PrepareContractTypeListModel(ContractTypeSearchModel searchModel);

        /// <summary>
        /// Prepare contract type model
        /// </summary>
        /// <param name="model">ContractType model</param>
        /// <param name="contractType">ContractType</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>ContractType model</returns>
        ContractTypeModel PrepareContractTypeModel(ContractTypeModel model, ContractType contractType, bool excludeProperties = false);
    }
}

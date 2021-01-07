using System.Collections;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Hotels;

namespace Nop.Services.Hotels
{
    /// <summary>
    /// ContractType service
    /// </summary>
    public partial interface IContractTypeService
    {
        #region ContractTypes

        /// <summary>
        /// Delete a contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        void DeleteContractType(ContractType contractType);

        /// <summary>
        /// Delete contractTypes
        /// </summary>
        /// <param name="contractTypes">ContractTypes</param>
        void DeleteContractTypes(IList<ContractType> contractTypes);

        /// <summary>
        /// Gets contractType
        /// </summary>
        /// <param name="contractTypeId">ContractType identifier</param>
        /// <returns>ContractType</returns>
        ContractType GetContractTypeById(int contractTypeId);

        /// <summary>
        /// Gets contractTypes by identifier
        /// </summary>
        /// <param name="contractTypeIds">ContractType identifiers</param>
        /// <returns>ContractTypes</returns>
        IList<ContractType> GetContractTypesByIds(int[] contractTypeIds);

        /// <summary>
        /// Gets all contract types
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ContractTypes</returns>
        IList<ContractType> GetAllContractTypes(bool showHidden = false);

        /// <summary>
        /// Gets all contract types
        /// </summary>
        /// <param name="contractTypeName">Contract Type name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ContractTypes</returns>
        IPagedList<ContractType> GetAllContractTypes(string contractTypeName,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<ContractType> SearchContractTypes(int pageIndex = 0,
            int pageSize = int.MaxValue,
            string keywords = null,
            int languageId = 0,
            bool showHidden = false,
            bool? overridePublished = null);

        /// <summary>
        /// Inserts a contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        void InsertContractType(ContractType contractType);

        /// <summary>
        /// Updates the contractType
        /// </summary>
        /// <param name="contractType">ContractType</param>
        void UpdateContractType(ContractType contractType);

        /// <summary>
        /// Updates the contractTypes
        /// </summary>
        /// <param name="contractTypes">ContractType</param>
        void UpdateContractTypes(IList<ContractType> contractTypes);

        #endregion
    }
}

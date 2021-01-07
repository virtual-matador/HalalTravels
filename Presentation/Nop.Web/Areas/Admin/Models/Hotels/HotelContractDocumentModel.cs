using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Hotels
{
    /// <summary>
    /// Represents a hotel contract document model
    /// </summary>
    public partial class HotelContractDocumentModel : BaseNopEntityModel
    {
        #region Properties

        public int HotelId { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.Document")]
        public int DocumentId { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.DocumentType")]
        public string DocumentType { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.Document")]
        public string DocumentUrl { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.OverrideAltAttribute")]
        public string OverrideAltAttribute { get; set; }

        [NopResourceDisplayName("Admin.Hotels.Hotel.ContractDocument.Fields.OverrideTitleAttribute")]
        public string OverrideTitleAttribute { get; set; }

        #endregion
    }
}
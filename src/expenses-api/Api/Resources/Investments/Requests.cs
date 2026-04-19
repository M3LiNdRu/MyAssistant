using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    [DataContract]
    public record PortfolioRequest
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [DataContract]
    public record InvestmentRequest
    {
        [Required]
        [DataMember(Name = "portfolioId")]
        public string PortfolioId { get; set; }

        [Required]
        [DataMember(Name = "assetType")]
        public string AssetType { get; set; }

        [Required]
        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [Required]
        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [Required]
        [DataMember(Name = "purchasePrice")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [DataMember(Name = "purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [DataMember(Name = "currentPrice")]
        public decimal CurrentPrice { get; set; }
    }

    [DataContract]
    public record UpdateInvestmentRequest
    {
        [Required]
        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [Required]
        [DataMember(Name = "currentPrice")]
        public decimal CurrentPrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
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

        [DataMember(Name = "strategy")]
        public List<PortfolioStrategy> Strategy { get; set; } = new();
    }

    [DataContract]
    public record InvestmentRequest
    {
        [Required]
        [DataMember(Name = "portfolioId")]
        public string PortfolioId { get; set; }

        [Required]
        [DataMember(Name = "assetType")]
        public string Type { get; set; }

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

    [DataContract]
    public record TransactionRequest
    {
        [Required]
        [DataMember(Name = "portfolioId")]
        public string PortfolioId { get; set; }

        [Required]
        [DataMember(Name = "type")]
        public TransactionType Type { get; set; }

        [Required]
        [DataMember(Name = "stock")]
        public Stock Stock { get; set; }

        [Required]
        [DataMember(Name = "totalAmount")]
        public Money TotalAmount { get; set; }

        [DataMember(Name = "fees")]
        public List<TransactionFee> Fees { get; set; } = new();

        [Required]
        [DataMember(Name = "broker")]
        public string Broker { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "notes")]
        public string Notes { get; set; }
    }
}

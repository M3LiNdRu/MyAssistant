using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    [DataContract]
    public record PortfolioResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    [DataContract]
    public record InvestmentResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "portfolioId")]
        public string PortfolioId { get; set; }

        [DataMember(Name = "assetType")]
        public string AssetType { get; set; }

        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [DataMember(Name = "purchasePrice")]
        public decimal PurchasePrice { get; set; }

        [DataMember(Name = "purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [DataMember(Name = "currentPrice")]
        public decimal CurrentPrice { get; set; }

        [DataMember(Name = "totalCost")]
        public decimal TotalCost => Quantity * PurchasePrice;

        [DataMember(Name = "totalValue")]
        public decimal TotalValue => Quantity * CurrentPrice;

        [DataMember(Name = "gainLoss")]
        public decimal GainLoss => TotalValue - TotalCost;

        [DataMember(Name = "gainLossPercent")]
        public decimal GainLossPercent => TotalCost > 0 ? (GainLoss / TotalCost) * 100 : 0;

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    [DataContract]
    public record PortfolioSummaryResponse
    {
        [DataMember(Name = "portfolioId")]
        public string PortfolioId { get; set; }

        [DataMember(Name = "totalInvestments")]
        public int TotalInvestments { get; set; }

        [DataMember(Name = "totalCost")]
        public decimal TotalCost { get; set; }

        [DataMember(Name = "totalValue")]
        public decimal TotalValue { get; set; }

        [DataMember(Name = "gainLoss")]
        public decimal GainLoss { get; set; }

        [DataMember(Name = "gainLossPercent")]
        public decimal GainLossPercent { get; set; }

        [DataMember(Name = "assetAllocation")]
        public Dictionary<string, AssetAllocationItem> AssetAllocation { get; set; }
    }

    [DataContract]
    public record TransactionResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "portfolio")]
        public PortfolioDto Portfolio { get; set; }

        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "assetType")]
        public string AssetType { get; set; }

        [DataMember(Name = "type")]
        public TransactionType Type { get; set; }

        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "notes")]
        public string Notes { get; set; }

        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    [DataContract]
    public record AssetAllocationItem
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public decimal Value { get; set; }

        [DataMember(Name = "percentage")]
        public decimal Percentage { get; set; }
    }
}

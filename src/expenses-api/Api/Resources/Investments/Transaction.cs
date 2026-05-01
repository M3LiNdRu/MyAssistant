using System;
using System.Collections.Generic;
using Library.MongoDb;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public class Transaction : ICollectionDocument
    {
        public string Id { get; set; }

        public PortfolioDto Portfolio { get; set; }

        public TransactionType Type { get; set; }

        public Stock Stock { get; set; }

        public Money TotalAmount { get; set; }

        public List<TransactionFee> Fees { get; set; } = new();

        public string Broker { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public enum TransactionType
    {
        Buy,
        Sell
    }

    public record Stock
    {
        public string Symbol { get; init; }
        public string Type { get; init; }
        public decimal Quantity { get; init; }
        public Money Price { get; init; }
    }

    public record Money
    {
        public decimal Amount { get; init; }
        public string CurrencyCode { get; init; } = "EUR";
    }

    public record TransactionFee
    {
        public string Description { get; init; }
        public Money Fee { get; init; }
    }
}

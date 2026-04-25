using System;
using Library.MongoDb;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public class Transaction : ICollectionDocument
    {
        public string Id { get; set; }

        public PortfolioDto Portfolio { get; set; }

        public string Symbol { get; set; }

        public string AssetType { get; set; }

        public TransactionType Type { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

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
}

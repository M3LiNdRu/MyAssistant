using System;
using System.Collections.Generic;
using Library.MongoDb;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public class Portfolio : ICollectionDocument
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PortfolioStrategy> Strategy { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public record PortfolioStrategy
    {
        public string StockType { get; init; }
        public string InformationDetails { get; init; }
    }

    public record PortfolioDto
    {
        public string Id { get; init; }
        public string Name { get; init; }
    }
}

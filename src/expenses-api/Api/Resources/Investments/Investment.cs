using System;
using Library.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public class Investment : ICollectionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PortfolioId { get; set; }

        public string AssetType { get; set; }

        public string Symbol { get; set; }

        public decimal Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal CurrentPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

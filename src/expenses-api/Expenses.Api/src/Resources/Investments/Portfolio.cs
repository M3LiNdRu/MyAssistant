using System;
using Library.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public class Portfolio : ICollectionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

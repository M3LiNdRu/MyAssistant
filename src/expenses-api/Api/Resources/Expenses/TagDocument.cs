using Library.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public class TagDocument : ICollectionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<string> Names { get; set; }
    }
}

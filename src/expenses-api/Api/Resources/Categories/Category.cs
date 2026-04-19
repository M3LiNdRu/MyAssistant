using Library.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{ 
    public class Category : ICollectionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}

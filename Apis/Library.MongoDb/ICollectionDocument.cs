using MongoDB.Bson.Serialization.Attributes;

namespace Library.MongoDb
{
    public interface ICollectionDocument
    {
        [BsonId]
        string Id { get; set; }
    }
}

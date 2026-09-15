using Library.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    public class Appointment : ICollectionDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

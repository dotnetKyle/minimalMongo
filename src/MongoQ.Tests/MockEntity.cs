using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoQ.Tests
{
    internal class MockEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}

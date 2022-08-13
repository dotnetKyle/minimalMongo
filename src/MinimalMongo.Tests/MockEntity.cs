using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalMongo.Tests
{
    internal class MockEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}

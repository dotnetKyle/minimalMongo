using MongoDB.Driver;

namespace MongoQ.Tests
{
    internal class MockRepo : MongoQRepo<MockEntity>
    {
        public MockRepo() : base("mockEntities") 
        { }

        public MongoCollectionSettings GetCollectionSettings()
            => _collectionSettings;

        public MongoDatabaseSettings GetDatabaseSettings()
            => _db.Settings;

        public MongoClientSettings GetClientSettings()
            => _client.Settings;

        public IMongoCollection<MockEntity> GetCollection()
            => _collection;
    }
}

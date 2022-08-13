using MongoDB.Driver;

namespace MinimalMongo.Tests
{
    internal class MockRepo : MinimalMongoRepo<MockEntity>
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

using System;
using MongoDB.Driver;

namespace MongoQ
{
    public class MongoQRepo<T> where T : class
    {
        protected IMongoClient _client { get; private set; }
        protected IMongoDatabase _db { get; private set; }
        protected IMongoCollection<T> _collection { get; private set; }
        protected MongoCollectionSettings _collectionSettings { get; private set; }

        public MongoQRepo(string collectionName, MongoCollectionSettings mongoCollectionSettings = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException(nameof(collectionName), "Collection name must contain a value");

            // these are stored in the appsettings
            var connString = MongoQConfiguration.MongoDbConnString;
            var dbName = MongoQConfiguration.MongoDbName;

            // create the client
            var settings = MongoClientSettings.FromConnectionString(connString);
            settings.ClusterConfigurator = MongoQConfiguration.ClusterConfig;
            _client = new MongoClient(settings);

            // get the database
            _db = _client.GetDatabase(dbName, MongoQConfiguration.MongoDbSettings);

            // get the collection for this repo
            _collection = _db.GetCollection<T>(dbName, mongoCollectionSettings);
        }
    }
}

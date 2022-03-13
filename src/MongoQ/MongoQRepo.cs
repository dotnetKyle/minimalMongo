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

        /// <summary>Filter Definition Builder</summary>
        protected FilterDefinitionBuilder<T> _filter => Builders<T>.Filter;
        /// <summary>Update Definition Builder</summary>
        protected UpdateDefinitionBuilder<T> _update => Builders<T>.Update;
        /// <summary>Sort Definition Builder</summary>
        protected SortDefinitionBuilder<T> _sort => Builders<T>.Sort;
        /// <summary>Projection Definition Builder</summary>
        protected ProjectionDefinitionBuilder<T> _projection => Builders<T>.Projection;
        /// <summary>Index Keys Definition Builder</summary>
        protected IndexKeysDefinitionBuilder<T> _indexKeys => Builders<T>.IndexKeys;

        public MongoQRepo(string collectionName, MongoCollectionSettings mongoCollectionSettings = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException(nameof(collectionName), "Collection name must contain a value");

            // these are stored in the appsettings
            var connString = MongoQConfiguration.MongoDbConnString;
            var dbName = MongoQConfiguration.MongoDbName;

            if (string.IsNullOrWhiteSpace(connString))
                throw new ArgumentNullException(nameof(MongoQConfiguration.MongoDbConnString), "You must configure a MongoDB connection string.");
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException(nameof(MongoQConfiguration.MongoDbName), "You must configure a MongoDB database name.");

            // create the client
            var settings = MongoClientSettings.FromConnectionString(connString);
            settings.ClusterConfigurator = MongoQConfiguration.ClusterConfig;
            _client = new MongoClient(settings);

            // get the database
            _db = _client.GetDatabase(dbName, MongoQConfiguration.MongoDbSettings);

            // get the collection for this repo
            _collection = _db.GetCollection<T>(collectionName, mongoCollectionSettings);
        }
    }
}

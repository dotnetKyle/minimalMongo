using System;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace MongoQ
{
    public static class MongoQConfiguration
    {
        /// <summary>
        /// From MongoDB docs: the ClusterConfigurator property is a delegate and 
        /// only its address is known for comparison. If you wish to construct 
        /// multiple MongoClients, ensure that your delegates are all using the 
        /// same address if the intent is to share connection pools.
        /// </summary>
        internal static Action<ClusterBuilder> ClusterConfig { get; private set; } = (ClusterBuilder cb) => { };
        internal static MongoDatabaseSettings MongoDbSettings { get; private set; }
        internal static string MongoDbName { get; private set; }
        internal static string MongoDbConnString { get; private set; }

        public static void AddMongoQDbName(string dbName)
        {
            if (string.IsNullOrWhiteSpace(dbName))
                throw new ArgumentNullException(nameof(dbName), "MongoDB Database name cannot be blank.");

            MongoDbName = dbName;
        }
        public static void AddMongoQConnectionString(string connstring)
        {
            if (string.IsNullOrWhiteSpace(connstring))
                throw new ArgumentNullException(nameof(connstring), "MongoDB Connection String cannot be blank.");

            MongoDbConnString = connstring;
        }

        public static void ReplaceClusterConfiguration(Action<ClusterBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "Config cannot be null");

            ClusterConfig = config;
        }

        public static void ReplaceDefaultDbSettings(Action<MongoDatabaseSettings> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Expression cannot be null");

            var mongoSettings = new MongoDatabaseSettings();

            options.Invoke(mongoSettings);

            MongoDbSettings = mongoSettings;
        }
    }
}

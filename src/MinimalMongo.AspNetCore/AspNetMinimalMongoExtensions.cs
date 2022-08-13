using Microsoft.AspNetCore.Builder;
using MongoDB.Driver.Core.Configuration;
using System;
using MongoDB.Driver;

namespace MinimalMongo.AspNetCore
{
    public static class AspNetMinimalMongoExtensions
    {
        /// <summary>
        /// Configure your mongodb cluster
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="options">This action will run when dependency injection is creating a new instance of a MongoRepo</param>
        public static IApplicationBuilder MinimalMongoConfigureCluster(this IApplicationBuilder app, Action<ClusterBuilder> options)
        {
            MinimalMongoConfiguration.ReplaceClusterConfiguration(options);

            return app;
        }

        /// <summary>
        /// Configure your mongodb database settings
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="options">This action will run when dependency injection is creating a new instance of a MongoRepo</param>
        public static IApplicationBuilder MinimalMongoConfigureDbSettings(this IApplicationBuilder app, Action<MongoDatabaseSettings> options)
        {
            MinimalMongoConfiguration.ReplaceDefaultDbSettings(options);

            return app;
        }

        /// <summary>
        /// Set the mongodb database name
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="dbName">The name of your mongodb database</param>
        public static IApplicationBuilder MinimalMongoSetDbName(this IApplicationBuilder app, string dbName)
        {
            MinimalMongoConfiguration.AddMinimalMongoDbName(dbName);

            return app;
        }

        /// <summary>
        /// Set the mongodb connection string
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="connString">The connection string for your mongodb database</param>
        public static IApplicationBuilder MinimalMongoSetConnString(this IApplicationBuilder app, string connString)
        {
            MinimalMongoConfiguration.AddMinimalMongoConnectionString(connString);

            return app;
        }
    }
}

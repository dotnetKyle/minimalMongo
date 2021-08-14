using Microsoft.AspNetCore.Builder;
using MongoDB.Driver.Core.Configuration;
using System;
using MongoDB.Driver;

namespace MongoQ.AspNetCore
{
    public static class AspNetMongoQExtensions
    {
        /// <summary>
        /// Configure your mongodb cluster
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="options">This action will run when dependency injection is creating a new instance of a MongoRepo</param>
        public static IApplicationBuilder MongoQConfigureCluster(this IApplicationBuilder app, Action<ClusterBuilder> options)
        {
            MongoQConfiguration.ReplaceClusterConfiguration(options);

            return app;
        }

        /// <summary>
        /// Configure your mongodb database settings
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="options">This action will run when dependency injection is creating a new instance of a MongoRepo</param>
        public static IApplicationBuilder MongoQConfigureDbSettings(this IApplicationBuilder app, Action<MongoDatabaseSettings> options)
        {
            MongoQConfiguration.ReplaceDefaultDbSettings(options);

            return app;
        }

        /// <summary>
        /// Set the mongodb database name
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="dbName">The name of your mongodb database</param>
        public static IApplicationBuilder MongoQSetDbName(this IApplicationBuilder app, string dbName)
        {
            MongoQConfiguration.AddMongoQDbName(dbName);

            return app;
        }

        /// <summary>
        /// Set the mongodb connection string
        /// </summary>
        /// <param name="app">Extension hangs off of the IApplicationBuilder to conform to ASP.NET Startup.cs conventions</param>
        /// <param name="connString">The connection string for your mongodb database</param>
        public static IApplicationBuilder MongoQSetConnString(this IApplicationBuilder app, string connString)
        {
            MongoQConfiguration.AddMongoQConnectionString(connString);

            return app;
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace MinimalMongo.Tests
{
    [TestClass]
    public class RepoTests
    {
        readonly string _defaultDbName;
        readonly string _defaultConnString;
        readonly MongoDatabaseSettings _defaultDbSettings;
        readonly Action<ClusterBuilder> _defaultClusterConfig;
        public RepoTests()
        {
            // get MinimalMongo defaults from the framework
            _defaultDbName = MinimalMongoConfiguration.MongoDbName;
            _defaultConnString = MinimalMongoConfiguration.MongoDbConnString;

            _defaultDbSettings = MinimalMongoConfiguration.MongoDbSettings?.Clone();
            _defaultClusterConfig = (MinimalMongoConfiguration.ClusterConfig != null)
                ? (Action<ClusterBuilder>)MinimalMongoConfiguration.ClusterConfig.Clone()
                : null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // reset MinimalMongo config back to the original defaults after each test
            MinimalMongoConfiguration.MongoDbName = _defaultDbName;
            MinimalMongoConfiguration.MongoDbConnString = _defaultConnString;
            MinimalMongoConfiguration.MongoDbSettings = _defaultDbSettings?.Clone();
            MinimalMongoConfiguration.ClusterConfig = (_defaultClusterConfig != null)
                ? (Action<ClusterBuilder>)_defaultClusterConfig.Clone()
                : null;
        }

        [TestMethod]
        public void NoConfiguration_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => {
                // setup mock repo without creating the MinimalMongo configuration first
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void NoDbName_ThrowsException()
        {
            // Arrange
            MinimalMongoConfiguration.MongoDbConnString = "mongodb://mongodb0.example.com:27017";

            Assert.ThrowsException<ArgumentNullException>(() => {
                // Act
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void NoConnString_ThrowsException()
        {
            // Arrange
            MinimalMongoConfiguration.MongoDbName = "fakeDbName";

            Assert.ThrowsException<ArgumentNullException>(() => {
                // Act
                // setup mock repo without creating the MinimalMongo configuration
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void WithConfigMinimum_NoExeptions()
        {
            // Arrange
            MinimalMongoConfiguration.AddMinimalMongoDbName("fakeDbName");
            MinimalMongoConfiguration.AddMinimalMongoConnectionString("mongodb://mongodb0.example.com:27017");

            // Act
            var repo = new MockRepo();
        }

        [TestMethod]
        public void RepoWithNullCollectionName_ThrowsException()
        {
            // Arrange
            MinimalMongoConfiguration.AddMinimalMongoDbName("fakeDbName");
            MinimalMongoConfiguration.AddMinimalMongoConnectionString("mongodb://mongodb0.example.com:27017");
            MockRepoWithoutCollectionName.CollectionName = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                var repo = new MockRepoWithoutCollectionName();
            });
        }

        [TestMethod]
        public void RepoWithEmptyCollectionName_ThrowsException()
        {
            // Arrange
            MinimalMongoConfiguration.AddMinimalMongoDbName("fakeDbName");
            MinimalMongoConfiguration.AddMinimalMongoConnectionString("mongodb://mongodb0.example.com:27017");
            MockRepoWithoutCollectionName.CollectionName = "";

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                // Act
                var repo = new MockRepoWithoutCollectionName();
            });
        }

        [TestMethod]
        public void EnsureCollectionNameGetSetRight()
        {
            // Arrange
            MinimalMongoConfiguration.AddMinimalMongoDbName("fakeDbName");
            MinimalMongoConfiguration.AddMinimalMongoConnectionString("mongodb://mongodb0.example.com:27017");

            var m = new MockRepo();

            var coll = m.GetCollection();

            var collName = coll.CollectionNamespace.CollectionName;
            
            Assert.AreEqual("mockEntities", collName, "Collection name was set wrong");
        }

        [TestMethod]
        public void EnsureDatabaseNameGetSetRight()
        {
            // Arrange
            MinimalMongoConfiguration.AddMinimalMongoDbName("fakeDbName");
            MinimalMongoConfiguration.AddMinimalMongoConnectionString("mongodb://mongodb0.example.com:27017");

            var m = new MockRepo();

            var coll = m.GetCollection();

            var dbName = coll.CollectionNamespace.DatabaseNamespace.DatabaseName;

            Assert.AreEqual("fakeDbName", dbName, "dbName was set wrong");
        }
    }
}

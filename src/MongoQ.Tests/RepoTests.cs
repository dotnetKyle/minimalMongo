using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace MongoQ.Tests
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
            // get mongoQ defaults from the framework
            _defaultDbName = MongoQConfiguration.MongoDbName;
            _defaultConnString = MongoQConfiguration.MongoDbConnString;

            _defaultDbSettings = MongoQConfiguration.MongoDbSettings?.Clone();
            _defaultClusterConfig = (MongoQConfiguration.ClusterConfig != null)
                ? (Action<ClusterBuilder>)MongoQConfiguration.ClusterConfig.Clone()
                : null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // reset mongoq config back to the original defaults after each test
            MongoQConfiguration.MongoDbName = _defaultDbName;
            MongoQConfiguration.MongoDbConnString = _defaultConnString;
            MongoQConfiguration.MongoDbSettings = _defaultDbSettings?.Clone();
            MongoQConfiguration.ClusterConfig = (_defaultClusterConfig != null)
                ? (Action<ClusterBuilder>)_defaultClusterConfig.Clone()
                : null;
        }

        [TestMethod]
        public void NoConfiguration_ThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => {
                // setup mock repo without creating the mongoq configuration first
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void NoDbName_ThrowsException()
        {
            // Arrange
            MongoQConfiguration.MongoDbConnString = "mongodb://mongodb0.example.com:27017";

            Assert.ThrowsException<ArgumentNullException>(() => {
                // Act
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void NoConnString_ThrowsException()
        {
            // Arrange
            MongoQConfiguration.MongoDbName = "fakeDbName";

            Assert.ThrowsException<ArgumentNullException>(() => {
                // Act
                // setup mock repo without creating the mongoq configuration
                var repo = new MockRepo();
            });
        }

        [TestMethod]
        public void WithConfigMinimum_NoExeptions()
        {
            // Arrange
            MongoQConfiguration.AddMongoQDbName("fakeDbName");
            MongoQConfiguration.AddMongoQConnectionString("mongodb://mongodb0.example.com:27017");

            // Act
            var repo = new MockRepo();
        }

        [TestMethod]
        public void RepoWithNullCollectionName_ThrowsException()
        {
            // Arrange
            MongoQConfiguration.AddMongoQDbName("fakeDbName");
            MongoQConfiguration.AddMongoQConnectionString("mongodb://mongodb0.example.com:27017");
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
            MongoQConfiguration.AddMongoQDbName("fakeDbName");
            MongoQConfiguration.AddMongoQConnectionString("mongodb://mongodb0.example.com:27017");
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
            MongoQConfiguration.AddMongoQDbName("fakeDbName");
            MongoQConfiguration.AddMongoQConnectionString("mongodb://mongodb0.example.com:27017");

            var m = new MockRepo();

            var coll = m.GetCollection();

            var collName = coll.CollectionNamespace.CollectionName;
            
            Assert.AreEqual("mockEntities", collName, "Collection name was set wrong");
        }

        [TestMethod]
        public void EnsureDatabaseNameGetSetRight()
        {
            // Arrange
            MongoQConfiguration.AddMongoQDbName("fakeDbName");
            MongoQConfiguration.AddMongoQConnectionString("mongodb://mongodb0.example.com:27017");

            var m = new MockRepo();

            var coll = m.GetCollection();

            var dbName = coll.CollectionNamespace.DatabaseNamespace.DatabaseName;

            Assert.AreEqual("fakeDbName", dbName, "dbName was set wrong");
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace MongoQ.Tests
{
    [TestClass]
    public class RepoTests
    {
        string _defaultDbName;
        string _defaultConnString;
        MongoDatabaseSettings _defaultDbSettings;
        Action<ClusterBuilder> _defaultClusterConfig;
        public RepoTests()
        {
            // get mongoQ defaults from the framework
            _defaultDbName = MongoQConfiguration.MongoDbName;
            _defaultConnString = MongoQConfiguration.MongoDbConnString;

            _defaultDbSettings = (MongoQConfiguration.MongoDbSettings != null) 
                ? MongoQConfiguration.MongoDbSettings.Clone()
                : null;
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
            MongoQConfiguration.MongoDbSettings = (_defaultDbSettings != null)
                ? _defaultDbSettings.Clone()
                : null;
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
    }
}

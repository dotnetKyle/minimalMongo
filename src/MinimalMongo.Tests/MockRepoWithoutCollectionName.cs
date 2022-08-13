namespace MinimalMongo.Tests
{
    internal class MockRepoWithoutCollectionName : MinimalMongoRepo<MockEntity>
    {
        public static string CollectionName = null;

        public MockRepoWithoutCollectionName() : base(CollectionName) { }

    }
}

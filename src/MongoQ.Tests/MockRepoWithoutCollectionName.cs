namespace MongoQ.Tests
{
    internal class MockRepoWithoutCollectionName : MongoQRepo<MockEntity>
    {
        public static string CollectionName = null;

        public MockRepoWithoutCollectionName() : base(CollectionName) { }

    }
}

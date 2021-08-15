namespace MongoQ.Tests
{
    internal class MockRepo : MongoQRepo<MockEntity>
    {
        public MockRepo() : base("mockEntities") { }
    }
}

# MinimalMongo

Quickly add the MongoDB C# Driver to a dotnet 
project with some helpful additions to get 
started faster.

## Usage:

Creating a repository is very easy with MinimalMongo. 
MinimalMongo will setup your collection for you in a 
standard, repeatable way so you can focus on your 
repository code.

```csharp
using MinimalMongo;
    
public class MyRepo : MinimalMongoRepo<MyEntity>
{
    public MyRepo() : base("collectionName") { }

    public MyEntity FindOne(ObjectId id)
    {
        return _collection
            .Find(e => e.Id == id)
            .FirstOrDefault();
    }
}
```

## Getting started

### Installation

Add the Nuget Package.

For ASP.NET:

`Install-Package MinimalMongo.AspNetCore`

Generic C#:

`Install-Package MinimalMongo`

### First, Configure your mongo database

See below, configuration is different per-environment to conform to conventions of your framework.

### Create a Repo

Inherit your repository from MinimalMongoRepo<T> where T is the class of your entity.

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
    
public class MyEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
}
public class MyRepo : MinimalMongo.MinimalMongoRepo<MyEntity>
{
    public MyRepo() : base("collectionName") { }

    public async Task<MyEntity> FindOne(ObjectId id)
    {
        return await _collection
            .Find(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
}
```
    
## Configuration

### AspnetCore Configuration

In your `Startup.cs` setup mongo in the `Startup.Configure` method using the `IApplicationBuilder app` parameter:

Set Database Name:

```csharp
app.MinimalMongoSetDbName("MyDatabase");
```

Set Connection string:

```csharp
app.MinimalMongoSetConnString(config.GetConnectionString("myConnString"));
```

Customize default database settings:

```csharp
app.MongoQConfigureDbSettings(options =>
{
    options.ReadPreference = ReadPreference.Nearest;
    options.WriteConcern = WriteConcern.WMajority;
    options.ReadConcern = ReadConcern.Majority;
});
```
    
Optionally, customize the cluster:

```csharp
app.MinimalMongoConfigureCluster(options =>
{
    // configure the connection pool
    options.ConfigureConnectionPool(
        (c) => new ConnectionPoolSettings(
            c.MaintenanceInterval,
            c.MaxConnections,
            c.MinConnections,
            c.WaitQueueSize,
            c.WaitQueueTimeout
        ));

    // subscribe to command failed events
    options.Subscribe<MongoDB.Driver.Core.Events.CommandFailedEvent>(e =>
    {
        System.Diagnostics.Debug.WriteLine($"Mongo Event:{e.GetType().Name}");
        System.Diagnostics.Debug.WriteLine($"CMD COMMAND FAILED:{e.CommandName}");
        System.Diagnostics.Debug.WriteLine($"CMD Exception:{e.Failure}");
    });
    // see generated commands in output
    options.Subscribe<MongoDB.Driver.Core.Events.CommandStartedEvent>(e =>
    {
        System.Diagnostics.Debug.WriteLine($"Mongo Event:{e.GetType().Name}");
        System.Diagnostics.Debug.WriteLine("CMD Start:" + e.Command?.ToJson());
        System.Diagnostics.Debug.WriteLine("CMD Name:" + e.CommandName);
    });
});
```

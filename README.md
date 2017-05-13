# FileStore  

<p align="center">
    <a href="https://ci.appveyor.com/project/mgibas/filestore/branch/master">
        <img src="https://ci.appveyor.com/api/projects/status/g5l1ixdibke1p5j3/branch/master?svg=true"></img>
    </a>
    <a href="https://www.gitcheese.com/donate/users/530319/repos/33446979">
        <img src="https://s3.amazonaws.com/gitcheese-ui-master/images/badge.svg"></img>
    </a>
    <a href="https://www.nuget.org/packages/NUnit.Asserts.Compare/">
        <img src="https://img.shields.io/nuget/v/NUnit.Asserts.Compare.svg?style=flat-square"></img>
    </a>
</p>

Simple .Net file storage

NuGet
====
```
Install-Package FileStore
Install-Package FileStore.Persistance.EntityFramework
Install-Package FileStore.Persistance.FileSystem
```

Features
====
* store files
* load files from store
* controll files versions

Usages
====
Versioned file store:
```csharp
var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
{
    cfg.UseFileSystem("myDirectory\\fileStoreDirectory");
});
var store = fileStoreBuilder.BuildVersioned();

var id = Guid.NewGuid();
store.StoreFile(id, new StoreFile { Data = new byte[] { 1, 2, 3 }, Extension = "txt" });
store.StoreFile(id, new StoreFile { Data = new byte[] { 1, 2, 3, 4, 5, 6 }, Extension = "txt" });

var latestFileVersion = store.OpenFile(id);
var firstVersion = store.OpenFile(id, 0);
```

Unversioned file store:
```csharp
var store = fileStoreBuilder.BuildUnversioned();
```
> With this setup You saves and opens files at single version - there is no ```store.OpenFile(Guid, int)```


Using Entity Framework:
```csharp
var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
{
  cfg.UseEntityFramework("some.connection.string.name");
  cfg.InitializeDatabase();
});
```

Registering in some container (Autofac in this example):
```csharp
var containerBuilder = new ContainerBuilder();
var fileStoreBuilder = FileStoreBuilder.Config(cfg =>
{
    cfg.UseFileSystem("C:\\fileStore");
});

containerBuilder.Register(c => fileStoreBuilder.BuildVersioned());

var container = containerBuilder.Build();
var store = container.Resolve<IFileStore>();
```
Contribute
====
Creating new Persistance engine is very simple - just implement `IPersistance` and provide configuration extensions. 

Maybe You have other idea - do no hesitate - please send me some pull request :)


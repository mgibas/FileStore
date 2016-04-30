# FileStore  [![gitcheese.com](https://img.shields.io/badge/gitcheese-donate-FDE383.svg?style=flat-square)](https://www.gitcheese.com/app/#/projects/d9efa213-d86a-49aa-8398-69a029901745/pledges/create) [![Build status](https://ci.appveyor.com/api/projects/status/g5l1ixdibke1p5j3)](https://ci.appveyor.com/project/mgibas/filestore)

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

====
[![gitcheese.com](https://api.gitcheese.com/v1/projects/d9efa213-d86a-49aa-8398-69a029901745/badges)](https://www.gitcheese.com/app/#/projects/d9efa213-d86a-49aa-8398-69a029901745/pledges/create)

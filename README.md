# <img src=".\docs\ModulR-logo.png" width="13%" height="13%"> ModulR

![Nuget](https://img.shields.io/nuget/v/ModulR?color=green&style=plastic)

NuGet package pages:
- [ModulR](https://www.nuget.org/packages/ModulR/)

ModulR is a factory pattern based, dependency injection module library to work with Microsoft's ServiceCollection. It combines the concepts of both the factory pattern and the module pattern (also available with the AutoFac dependency injection library) and creates a solution that provides more detailed control on how the dependencies are registered and accessed by the IoC container. 

It is setup in a way that the container can find the related dependencies registered in a particular module. The library utilizes the main service/client/dependency that separates one module's domain from another as the access point. Therefore it does not encourage to use anti-patterns like the service-locator pattern. However, it provides the possibility to use a factory, which works as some sort of service locator where a key is used to find the right service implemented by the right module. Nevertheless The factory is scoped to only a single type of dependency and to get a specific version of that dependency from a module. In short, you can access a specific version of the dependency from a particular module by choosing that module by a key.

| :exclamation: **Note**                                       |
| :----------------------------------------------------------- |
| From version 2.0.0 onward the *Module* abstract class implementation uses custom a service-collection and service-provider which discards the tedious extra dependency registrations that are needed to get the dependencies that otherwise registered by other service-collections. So the *.AddModule<TModule>(...)*-like extensions do not have to be called in every module to get shared dependencies. However, the main service-collection (in the DI main) still has to do this, because it otherwise does not know where to get the dependencies from that originate from modules. This new feature actually uses the main service-provider from the main service-collection, so it is recommended to register the modules and certain dependencies from other modules in the main DI. What is also possible is that when the main service-collection only adds the modules, then the modules themselves only need to use a *FromModule<TModule>(...)* call to get the requested dependencies. |



## Setting up the module

We start with defining a module. In this case it we create a module that concerns about ordering.

```c#
public class OrderingModule : Module
{
    protected override void Configure(IServiceCollection services)
    {
        services
            .AddTransient<ISharedService, SharedService>()
            .AddTransient<IThirdPartyOrderClient, ThirdPartyOrderClient>()
            .Configure<ThirdPartyOrderClientOptions>(options =>
            {
                options.Endpoint = new Uri(new Uri(base.Configuration["ThirdParty:BaseEndpoint"]), "/order/");
                options.UserCredentialsRetrieverBaseUrl = base.Configuration["ThirdParty:KeyVaultBaseAddress"];
            });
        
        services
            .AddHttpClient<IOwnOrderService, OwnOrderService>()
            .WithHttpRequestHandler<AuthorizationHeaderHandler>()
            .WithPolicy(PollyPolicyEngine.CreateOrderingPolicy());
    }
}
```

This module can be registered simply by using the .AddModule<TModule>() extension for IServiceCollection.

```c#
var services = new ServiceCollection();

// This is going to work, because of the required configuration
services.AddModule<OrderingModule>(configuration);

// This will work in other instances but it will here throw an exception eventually, because the OrderingModule needs the configuration
services.AddModule<OrderingModule>();
```

Notice that the OrderingModule needs an IConfiguration instance as it uses it to access settings (in the example it regards the calls to base.Configuration[...]). The Module abstract class provides the .WithConfiguration(IConfiguration configuration) method that can be used to provide the IConfiguration instance. However, the registration extensions can do this for you. When you need to use this, you can simply provide it to the extension method.

| :exclamation: **Note**                                       |
| :------------------------------------------------------------ |
| With the .AddModule<TModule>() and .AddModule<TModule>(IConfiguration) methods, the only thing that is registered is a singleton instance of the module. Now you might question its use-case, but this type of registration is useful for other cases that this library provides and will be touched upon further down this document. |

We are now going to define an access point to a service/client which is made available by the module. We will show two cases.

```c#
// Case 1:
services
    .AddModularClient<IOwnOrderService>()
    .From<OrderingModule>(configuration);
```

```c#
// Case 2:
services
    .AddModule<OrderingModule>(configuration)
    .AddModularClient<IOwnOrderService>()
    .From<OrderingModule>();
```

First, let *Case 1* and *Case 2* both be separate executions which both use a clean instance of IServiceCollection. Then, we can observe the cases and explain what is happening in both of them.

In the context of *Case 1*, OrderingModule is not yet registered, though, will be done by the .From<TModule>(...) method. That is also why the IConfiguration instance needs to be passed down into this method. Now when stumbling upon the IOwnOrderService instance in the main dependency injection registration location of the application, we will see that the service is retrieved from the OrderingModule and all the dependencies of OwnOrderService, and the dependencies of these dependencies and so on, that are registered in that module. 

In the context *Case 2*, OrderingModule is first registered (with the configuration) and where after next the client will be registered that comes from the module. However, now the configuration input is no longer needed in the .From<TModule>(...) method, because the .AddModule<TModule>(...) method has already provided this.

| :exclamation: **Note**                                       |
| :------------------------------------------------------------ |
| When the .AddModularClient extension is used twice in a row for the same client/service, the main DI service-provider will always return the instance of that client/service that was registered last. This has nothing to do with the library, but rather with how IServiceCollection works. By default, IServiceCollection can only register one instance of a particular type of service/client at the time. Nevertheless, all the modules that are referenced with all the times that .AddModuleClient is used in the main DI and followed directly up by calling the .From<TModule>() method, will all be registered when not already existing. |

| :exclamation: **Note**                                       |
| :------------------------------------------------------------ |
| There is also an .AddModularClient<TService, TImplementation>() method, which registers a specific version of the *TService* in the form of *TImplementation*. |

Finally there is the possibility to create a factory that can provide a specific version of a specified service/client/dependency by the means of a key, which corresponds to a specific module.

```c#
services
    .AddModule<ArticleModule>()
    .AddModulRFactoryForService<string, ISharedService>(registry =>
    {
        registry
            .OnKey("Order").FromModule<OrderingModule>()
            .OnKey("Article").FromModule<ArticleModule>();
    });
```

The .AddModulRFactoryForService<TKey, TService>(...) method provides the possibility to define a builder to create the structure to provide multiple versions of the ISharedService through these modules. In this example, both the versions are registered, one in the OrderingModule and one in the ArticleModule. By the means of a dedicated key you can select the corresponding module and get the desired version of ISharedService.

| :exclamation: **Note**                                       |
| :------------------------------------------------------------ |
| The .AddModule<ArticleModule>() method is required to register the module and make it available for the registry. Within this example, we assume that we already registered the OrderingModule, though, not explicitly visible in this example. |

A IModulRServiceProviderFactory<TKey, TService> instance is created that can be used to get the right version of the service.


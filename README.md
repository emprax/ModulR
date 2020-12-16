# <img src=".\docs\ModulR-logo.png" style="zoom:13%;" />ModulR
ModulR is a factory pattern based, dependency injection module library to work with Microsoft ServiceCollection. It merges the concepts of the factory pattern and the module pattern (also available with the AutoFac dependency injection library) into a solution that provides more detailed control to how the dependencies are registered and accessed regarding your IoC container. 

It is setup in a way that the container can find the regarding dependencies registered in a particular module by which it uses the main service/client/dependency that separates one module's domain from another as the access point. Therefore it does not encourage to use anti-patterns like the service-locator pattern. However, it provides the possibility to use a key on a factory, scoped to only a single type of dependency, to get a specific version of that dependency from a module. In short, you can access a specific version of the dependency from a particular module by choosing that module by the provided key.



## Setting up the module

We start with defining a module, in this case it regards a module that concerns about ordering.

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

Notice that the OrderingModule needs an IConfiguration instance as it uses that to access settings (in the example it regards the calls to base.Configuration[...]). The Module abstract class provides the .WithConfiguration(IConfiguration configuration) method that can be used to provide the IConfiguration instance. However, the registration extensions can do this for you. When you need to use this, you can simply provide it to the extension method.

**Note:** *With the .AddModule<TModule>() and .AddModule<TModule>(IConfiguration) methods, the only thing that is registered is a singleton instance of the module, there is no access point. Now you might question its use-case, but this type of registration is useful for other registration cases that this library provides and will be discussed further down this document.*

We are now going to define an access point to a service/client which is made available by the module. We will show two cases.

```c#
// Case 1:
services
    .AddModularClient<IOwnOrderService>()
    .From<OrderingModule>(configuration);

// Case 2:
services
    .AddModule<OrderingModule>(configuration)
    .AddModularClient<IOwnOrderService>()
    .From<OrderingModule>();
```

When regarding that *Case 1* and *Case 2* are both separate executions that both use a clean instance of IServiceCollection, then we can observe these cases and explain what is happening in both of them.

In the case of *Case 1*, OrderingModule is not yet registered and will be done by the .From<TModule>(...) method, that is also why the IConfiguration instance needs to be passed down into this method. Now when stumbling upon the IOwnOrderService instance in the main section, it will retrieve it from the OrderingModule and all the dependencies of OwnOrderService, and the dependencies of these dependencies and so on that are registered in that module. 

In the case of *Case 2*, OrderingModule is first registered (with the configuration) and then it will register the client from the module, however, now the configuration input is no longer needed in the .From<TModule>(...) method as the .AddModule<TModule>(...) method has already provided this.

**Note:** *There is also an .AddModulareClient<TService, TImplementation>() method, which registers a specific version of the TService in the form of TImplementation.*

Finally there is the possibility to create a factory that can provide a specific version of a specified service/client/dependency by the means of a key, which corresponds to specific module.

```c#
services
    .AddModule<ArticleModule>()
    .AddModulRFactoryForService<string, ISharedService>((key, registry) =>
    {
        return registry
            .OnKey("Order").FromModule<OrderingModule>()
            .OnKey("Article").FromModule<ArticleModule>()
            .Provide(key);
    });
```

The .AddModulRFactoryForService<TKey, TService>(...) method provides the possibility to define a builder to construct the tree-way to possible versions of the ISharedService by which versions are both registered in the OrderingModule and the ArticleModule. By the means of a dedicated key you can select the corresponding module and get the desired version of ISharedService.

**Note:** *The .AddModule<ArticleModule>() method is needed to register the module and make it available for the registry. We assume that we already registered the OrderingModule.*

A IModulRServiceProviderFactory<TKey, TService> instance is created that can be used to get the right version of the service.


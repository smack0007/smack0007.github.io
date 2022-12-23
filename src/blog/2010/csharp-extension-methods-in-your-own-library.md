---
Title: C# Extension Methods in your own Library
Date: 2010-03-08
Tags: .net, c#, extension-methods 
---

Normally I use extension methods in C# to extend a library that I did not write and therefore I have no control over. There are situations where it makes sense to use extension methods for a library that you yourself are writing.

For example, when you have interfaces in your library. You want to keep the number of methods in that interface as low as possible so that classes implementing the interface don't have to do a lot of heavy lifting. This means cutting out methods in an interface that are for the most part just syntactic sugar for another method in the interface.

```c#

public interface IServiceContainer
{
    void AddService(Type type, Object provider);
    object GetService(Type type);
}

public static class IServiceContainerExtensions
{
    public static void AddService&lt;T&gt;(this IServiceContainer services, object provider)
    {
        services.AddService(typeof(T), provider);
    }

    public static T GetService&lt;T&gt;(this IServiceContainer services) where T : class
    {
        return services.GetService(typeof(T)) as T;
    }

    public static T GetRequiredService&lt;T&gt;(this IServiceContainer services) where T : class
    {
        T service = services.GetService(typeof(T)) as T;

        if(service == null)
            throw new ServiceNotFoundException(typeof(T));

        return service;
    }
}

```

All of the methods in IServiceContainerExtensions are just helper methods for method in IServiceContainer. By making them extension methods in our own library though, we've made the barrier to entry lower. Other people can implement the interface and in a sense "inherit" the helper methods as well.

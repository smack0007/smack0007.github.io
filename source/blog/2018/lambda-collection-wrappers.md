---
Title: Lambda Collection Wrappers
Subtitle: 
Date: 2018-04-26
Category: c#
Tags: c#, lambda, fp
---

I've been reading a lot as of late about functional programming and I try to
incorperate as much as possible into my everyday programming. One trick that
I've definitely started using is wrapping collections in lambda functions.

<!--more-->

I'll try to explain with an example:

```csharp
static void Main(string[] args)
{
    var typesToAssemblies = new Dictionary<Type, Assembly>();
    
    void BuildMap(Assembly assembly)
    {
        foreach (var type in assembly.DefinedTypes)
            typesToAssemblies[type.AsType()] = type.Assembly;

        foreach (var reference in assembly.GetReferencedAssemblies())
            BuildMap(Assembly.Load(reference));
    }

    BuildMap(Assembly.GetEntryAssembly());

    DisplayType(typesToAssemblies, typeof(string));
    DisplayType(typesToAssemblies, typeof(Enumerable));
    DisplayType(typesToAssemblies, typeof(TypeInfo));
    DisplayType(typesToAssemblies, typeof(Dictionary<,>));
}

private static void DisplayType(Dictionary<Type, Assembly> typesToAssemblies, Type type)
{
    if (typesToAssemblies.TryGetValue(type, out var assembly))
    {
        Console.WriteLine($"{type}: <{assembly}>");
    }
    else
    {
        Console.WriteLine($"{type} is not available.");
    }
}
```

The function `DisplayType` takes a `Dictionary<Type, Assembly>` as it's first parameter although
it really only needs some function to look up if the type exists. This is evident by the fact that
`TryGetValue` is the only method used from the dictionary. Let's refactor.

```csharp
static void Main(string[] args)
{
    var typesToAssemblies = new Dictionary<Type, Assembly>();
    
    void BuildMap(Assembly assembly)
    {
        foreach (var type in assembly.DefinedTypes)
            typesToAssemblies[type.AsType()] = type.Assembly;

        foreach (var reference in assembly.GetReferencedAssemblies())
            BuildMap(Assembly.Load(reference));
    }

    BuildMap(Assembly.GetEntryAssembly());

    Func<Type, Assembly> getAssembly = x =>
    {
        if(typesToAssemblies.TryGetValue(x, out var assembly))
        {
            return assembly;
        }

        return null;
    };

    DisplayType(getAssembly, typeof(string));
    DisplayType(getAssembly, typeof(Enumerable));
    DisplayType(getAssembly, typeof(TypeInfo));
    DisplayType(getAssembly, typeof(Dictionary<,>));
}

private static void DisplayType(Func<Type, Assembly> getAssembly, Type type)
{
    var assembly = getAssembly(type);
    if (assembly != null)
    {
        Console.WriteLine($"{type}: <{assembly}>");
    }
    else
    {
        Console.WriteLine($"{type} is not available.");
    }
}
```

This version produces the same output as the previous version. You may be thinking to yourself:
Great but you just shuffled some code around. Yes and no. `DisplayType` is no longer dependent on
the type `Dictionary<Type, Assembly>`. We could in theory, build the `typesToAssemblies` dictionary
lazily.

```csharp
static void Main(string[] args)
{
    Dictionary<Type, Assembly> typesToAssemblies = null;
    
    void BuildMap(Assembly assembly)
    {
        foreach (var type in assembly.DefinedTypes)
            typesToAssemblies[type.AsType()] = type.Assembly;

        foreach (var reference in assembly.GetReferencedAssemblies())
            BuildMap(Assembly.Load(reference));
    }

    Func<Type, Assembly> getAssembly = x =>
    {
        if (typesToAssemblies == null)
        {
            typesToAssemblies = new Dictionary<Type, Assembly>();
            BuildMap(Assembly.GetEntryAssembly());
        }

        if(typesToAssemblies.TryGetValue(x, out var assembly))
        {
            return assembly;
        }

        return null;
    };

    DisplayType(getAssembly, typeof(string));
    DisplayType(getAssembly, typeof(Enumerable));
    DisplayType(getAssembly, typeof(TypeInfo));
    DisplayType(getAssembly, typeof(Dictionary<,>));
}

private static void DisplayType(Func<Type, Assembly> getAssembly, Type type)
{
    var assembly = getAssembly(type);
    if (assembly != null)
    {
        Console.WriteLine($"{type}: <{assembly}>");
    }
    else
    {
        Console.WriteLine($"{type} is not available.");
    }
}
```

By using a lambda to wrap the collection, we have made the `DisplayType` function
more flexible. If we decide that `Dictionary<Type, Assembly>` is the wrong collection
type for our use case at some point, `DisplayType` will not be affected as long as
you can give it a function with the same interface.
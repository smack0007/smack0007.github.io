---
Title: Abusing IDisposable
Subtitle: 
Date: 2019-08-27
Category: c#
Tags: c#, IDisposable
---

In C# the interface `IDisposable` is usually used to indicate that an object
needs to dispose of unmanaged resources. C# provides the `using` syntax to
ensure that the `Dispose` method is called once the instance of the object is
no longer needed.

`Dispose` is just like any other method though and the `using` syntax can
be used quite nicely for other use cases as well. Let's create a class we
can use to collect some actions to be "deferred" until a later time.

<!--more-->

```c#
class DeferredActions : IDisposable
{
    private List<Action> _actions = new List<Action>();

    public void Push(Action action) => _actions.Add(action);

    void IDisposable.Dispose()
    {
        foreach (var action in _actions)
            action();

        _actions.Clear();
    }
}

class Program
{
    public static void Main(string[] args)
    {
        using (var deferredActions = new DeferredActions())
        {
            deferredActions.Push(() => Console.WriteLine("Deferred Action 1"));
            Console.WriteLine("Doing work...");
            deferredActions.Push(() => Console.WriteLine("Deferred Action 2"));
            Console.WriteLine("Doing more work...");
            deferredActions.Push(() => Console.WriteLine("Deferred Action 3"));
        }
    }
}
```

The output is of course:

```
Doing work...
Doing more work...
Deferred Action 1
Deferred Action 2
Deferred Action 3
```

Here we're using the `using` syntax to indicate when our deferred actions should
be executed. We can also use an `IDisposable` object in multiple `using` blocks:

```c#
public static void Main(string[] args)
{
    var deferredActions = new DeferredActions();

    using (deferredActions)
    {
        deferredActions.Push(() => Console.WriteLine("Deferred Action 1"));
        Console.WriteLine("Doing work...");
        deferredActions.Push(() => Console.WriteLine("Deferred Action 2"));
        Console.WriteLine("Doing more work...");
        deferredActions.Push(() => Console.WriteLine("Deferred Action 3"));
    }

    using (deferredActions)
    {
        deferredActions.Push(() => Console.WriteLine("Deferred Action 4"));
        Console.WriteLine("Reticulating splines...");
        deferredActions.Push(() => Console.WriteLine("Deferred Action 5"));
    }
}
```

And the output:

```
Doing work...
Doing more work...
Deferred Action 1
Deferred Action 2
Deferred Action 3
Reticulating splines...
Deferred Action 4
Deferred Action 5
```
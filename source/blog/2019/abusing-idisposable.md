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
class DeferedActions : IDisposable
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
        using (var deferedActions = new DeferedActions())
        {
            deferedActions.Push(() => Console.WriteLine("Defered Action 1"));
            Console.WriteLine("Doing work...");
            deferedActions.Push(() => Console.WriteLine("Defered Action 2"));
            Console.WriteLine("Doing more work...");
            deferedActions.Push(() => Console.WriteLine("Defered Action 3"));
        }
    }
}
```

The output is of course:

```
Doing work...
Doing more work...
Defered Action 1
Defered Action 2
Defered Action 3
```

Here we're using the `using` syntax to indicate when our deferred actions should
be executed. We can also use an `IDisposable` object in multiple `using` blocks:

```c#
public static void Main(string[] args)
{
    var deferedActions = new DeferedActions();

    using (deferedActions)
    {
        deferedActions.Push(() => Console.WriteLine("Defered Action 1"));
        Console.WriteLine("Doing work...");
        deferedActions.Push(() => Console.WriteLine("Defered Action 2"));
        Console.WriteLine("Doing more work...");
        deferedActions.Push(() => Console.WriteLine("Defered Action 3"));
    }

    using (deferedActions)
    {
        deferedActions.Push(() => Console.WriteLine("Defered Action 4"));
        Console.WriteLine("Reticulating splines...");
        deferedActions.Push(() => Console.WriteLine("Defered Action 5"));
    }
}
```

And the output:

```
Doing work...
Doing more work...
Defered Action 1
Defered Action 2
Defered Action 3
Reticulating splines...
Defered Action 4
Defered Action 5
```
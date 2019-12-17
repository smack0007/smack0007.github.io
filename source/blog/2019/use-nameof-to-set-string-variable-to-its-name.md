---
Title: Use nameof() to set a string variable to its name
Subtitle: 
Date: 2019-12-17
Category: c#
Tags: c#, nameof()
---

This is one of those things that should have just been obvious to me
but once I saw it I wondered why I had never thought to use it myself.
`nameof()` can be used when declaring a variable to set the value of the
variable to the name of the variable:


```c#
public const string MyVariable1 = nameof(MyVariable1);

public static void Main()
{
    Console.WriteLine(MyVariable1);
}
```

<!--more-->

This feels similar to the trick of using a class as a generic parameter in the
declaration of the class:

```c#
class Base<T>
{
    public void WriteClassName() => Console.WriteLine(typeof(T).Name);
}

class Foo : Base<Foo>
{
}

public static void Main()
{
    var obj = new Foo();
    obj.WriteClassName();
}
```

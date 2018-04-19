---
Title: string.Join Extension Method
Subtitle: 
Date: 2018-04-19
Category: c#
Tags: c#, linq
---

Normally if want to do a `string.Join` on the result of a Linq query you end up with somthing looking like
this:

```csharp
Console.WriteLine(string.Join(", ", numbers.Where(x => x % 2 == 0)));
```

The call to `string.Join` has to come first and then the Linq query. I've always felt this breaks the flow
of the code and would be easier to read if the `string.Join` was at the end of the Linq query:

```csharp
Console.WriteLine(numbers.Where(x => x % 2 == 0).JoinString(", "));
```

This can be implemented with the following extension methods:

```csharp
public static class JoinStringExtensions
{
    public static string JoinString<T>(this IEnumerable<T> source, string seperator) =>
        string.Join(seperator, source.Select(x => x.ToString()));

    public static string JoinString(this IEnumerable<string> source, string seperator) =>
        string.Join(seperator, source);
}
```

The specialization for `IEnumerable<string>` is just mirroring the implementation from `string.Join`.


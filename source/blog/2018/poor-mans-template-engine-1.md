---
Title: Poor Man's Template Engine in C# Part 1
Subtitle: Groundwork
Date: 2018-03-06
Category: c#
Tags: c#, template, template-engine
---

If you're looking for a poor man's solution to a templating engine for .net and don't really need
the overhead a complete template engine brings with it, I've come up with the following solution.

<!--more-->

```c#
namespace Pmte
{
    public delegate IEnumerable<string> Template<TData>(TData data);

    public static class TemplateExtensions
    {   
        public static string Render(this IEnumerable<string> templateResult) => string.Join(Environment.NewLine, templateResult);
    }
}
```

We define a template as being any function that returns an `IEnumerable<string>`. We are going to build a functional style
template engine. TemplateExtensions will contain methods for working with templates and template results.

Implement a template like so:

```c#
using static Pmte.TemplateExtensions;

public static class Templates
{
    public static IEnumerable<string> PeopleList(IEnumerable<Person> people)
    {
        foreach (var person in people)
            yield return $"{person.FirstName} {person.LastName}";
    }
}
```

This can then be consumed with:

```c#
var people = new Person[]
{
    new Person() { FirstName = "Stan", LastName = "Marsh" },
    new Person() { FirstName = "Kyle", LastName = "Broflovski" },
    new Person() { FirstName = "Kenny", LastName = "McCormick" },
    new Person() { FirstName = "Eric", LastName = "Cartman" },
};

Console.WriteLine(Templates.PeopleList(people).Render());
```

In future episodes we will build more functions to make our template engine more powerful.

---
Title: Poor Man's Template Engine in C# Part 1
Subtitle: Groundwork
Date: 2018-03-06
Category: c#
Tags: c#, template, template-engine
---

If you're looking for a poor man's solution to a templating engine for .net and don't really need
the overhead a complete template engine brings with it, I've come up with the following solution:

```c#
public abstract class Template<T>
{
    public string Render(T data)
    {
        return string.Join(Environment.NewLine, RenderTemplate(data));
    }

    protected abstract IEnumerable<string> RenderTemplate(T data);
}

public class PeopleTemplate : Template<IEnumerable<Person>>
{
    protected override IEnumerable<string> RenderTemplate(IEnumerable<Person> people)
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

var template = new PeopleTemplate();
Console.WriteLine(template.Render(people));
```

This is a really barebones solution but it works. In future posts I will build on
this idea further to make our templating solution more powerful.

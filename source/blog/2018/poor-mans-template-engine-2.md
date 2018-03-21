---
Title: Poor Man's Template Engine in C# Part 2
Subtitle: HtmlEncode and If
Date: 2018-03-14
Category: c#
Tags: c#, template, template-engine
---

In our [last episode]({{baseUrl}}/blog/2018/poor-mans-template-engine-1.html) we laid the groundwork for our
simple template engine. In this episode we'll introduce our first 2 helper functions:

```c#
namespace Pmte
{
    public delegate IEnumerable<string> Template<TData>(TData data);

    public static class TemplateExtensions
    {   
        public static string Render(this IEnumerable<string> templateResult) => string.Join(Environment.NewLine, templateResult);

        public static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        public static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
    }
}
```

The first helper function `HtmlEncode` is fairly self explanitory. Use it to encode a string so that it is valid html.

`If` can be used inside an interpolated string to optionally include a string or include a different string depending on
if condition is true or false.

```c#
public static class Templates
{
    public static IEnumerable<string> Path(string[] paths)
    {
        string output = "";

        for (int i = 0; i < paths.Length; i++)
            output += $"{paths[i]}{If(i < paths.Length - 1, HtmlEncode(" >"))}";

        yield return output;
    }
}
```

Next time we'll talk about including templates inside of other templates.

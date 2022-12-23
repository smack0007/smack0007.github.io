---
Title: Poor Man's Template Engine in C# Part 3
Subtitle: Include
Date: 2018-03-22
Tags: c#, template, template-engine
---

In our [last episode](blog/2018/poor-mans-template-engine-2.html) we implemeted the first
two helper funcitons `HtmlEncode` and `If`. Today we want to implement `Include`.

<!--more-->

```c#
namespace Pmte
{
    public delegate IEnumerable<string> Template<TData>(TData data);

    public static class TemplateExtensions
    {   
        public static string Render(this IEnumerable<string> templateResult) => string.Join(Environment.NewLine, templateResult);

        public static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        public static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;

        public static string Include<TData>(Template<TData> template, TData data) =>
            template(data).Render();
    }
}
```

Include can be used by specifying the template function and the data used to call the template function. The rendered result is returned.

Example usage:

```c#
using static Pmte.TemplateExtensions;

public static class Templates
{
    public static IEnumerable<string> Post(PostData post)
    {
        yield return "<div class=\"post\">";
            yield return Include(PostHeader, post);
            yield return "<div class=\"content\">";
                yield return post.Content;
            yield return "</div>";
        yield return "</div>";
    }
    
    public static IEnumerable<string> PostHeader(PostData post)
    {
        yield return $"<h2><a href=\"{post.Url}\">{HtmlEncode(post.Title)}</a></h2>";   

        yield return "<div class=\"meta\">";
            yield return $"<span class=\"date\">{post.Date}</span>";
            yield return $"<span class=\"category\">{HtmlEncode(post.Category)}</span>";
        yield return "</div>";
    }

    public static IEnumerable<string> PostsList(IEnumerable<PostData> posts)
    {
        yield return "<div class=\"posts\">";

        foreach (var post in posts)
        {
            yield return "<div class=\"post\">";
                yield return Include(PostHeader, post);
            yield return "</div>";
        }

        yield return "</div>";
    }
}
```

We reuse the `PostHeader` template in both the `Post` and `PostsList` templates.
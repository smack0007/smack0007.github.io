using System;
using System.Collections.Generic;

namespace compiler
{
    public class IndexTemplate : Template<IndexData>
    {
        protected override IEnumerable<string> RenderTemplate(IndexData data)
        {
            foreach (var post in data.Posts)
            {
                yield return "<div class=\"post\">";
                    yield return $"<h2><a href=\"{post.Path}\">{HtmlEncode(post.Title)}</a></h2>";   
                    yield return "<div class=\"meta\">";
                        yield return $"<span class=\"date\"><i class=\"fa fa-calendar\"></i>{post.Date}</span>";
                        yield return $"<span class=\"category\"><i class=\"fa fa-tags\"></i>{HtmlEncode(post.Category)}</span>";
                    yield return "</div>";
                    yield return "<div class=\"content\">";
                        yield return post.Excerpt;
                    yield return "</div>";
                yield return "</div>";
            }
        }
    }
}
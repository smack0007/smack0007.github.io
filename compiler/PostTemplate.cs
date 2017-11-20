using System;
using System.Collections.Generic;

namespace compiler
{
    public class PostTemplate : Template<PostData>
    {
        protected override IEnumerable<string> RenderTemplate(PostData data)
        {
            yield return "<div class=\"post\">";
                yield return Include(PostTemplate.Header, data);
                yield return "<div class=\"content\">";
                    yield return data.Content;
                yield return "</div>";
            yield return "</div>";
        }

        public static IEnumerable<string> Header(PostData data)
        {
            yield return $"<h2><a href=\"{data.Path}\">{HtmlEncode(data.Title)}</a></h2>";   
            yield return "<div class=\"meta\">";
                yield return $"<span class=\"date\"><i class=\"fa fa-calendar\"></i>{data.Date}</span>";
                yield return $"<span class=\"category\"><i class=\"fa fa-folder\"></i>{HtmlEncode(data.Category)}</span>";
                foreach (var tag in data.Tags)
                {
                    yield return $"<span class=\"tags\"><i class=\"fa fa-tags\"></i>{HtmlEncode(tag)}</span>";
                }
            yield return "</div>";
        }
    }
}
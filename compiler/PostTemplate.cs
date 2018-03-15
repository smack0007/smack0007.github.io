using System;
using System.Collections.Generic;
using System.Linq;

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
                yield return $"<span class=\"date\"><span class=\"fas fa-calendar-alt\"></span>{data.Date}</span>";
                yield return $"<span class=\"category\"><span class=\"fas fa-folder\"></span>{HtmlEncode(data.Category)}</span>";
                var tags = data.Tags.Select(x => HtmlEncode(x));
                yield return $"<span class=\"tags\"><span class=\"fas fa-tags\"></span>{string.Join(", ", tags)}</span>";
            yield return "</div>";

            if (!string.IsNullOrEmpty(data.Subtitle))
                yield return $"<h3>{HtmlEncode(data.Subtitle)}</h3>";   
        }
    }
}
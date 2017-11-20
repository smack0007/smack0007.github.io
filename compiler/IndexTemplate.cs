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
                    yield return Include(PostTemplate.Header, post);
                    yield return "<div class=\"content\">";
                        yield return post.Excerpt;
                    yield return "</div>";
                yield return "</div>";
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace compiler
{
    public class PostTemplate : Template<PostData>
    {
        protected override IEnumerable<string> RenderTemplate(PostData data)
        {
            yield return $"<h2>{HtmlEncode(data.Title)}</h2>";
            yield return $"{data.Content}";
        }
    }
}
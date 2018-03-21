using System.Collections.Generic;
using System.Linq;
using static compiler.Template;

namespace compiler
{
    public static class Templates
    {
        public static IEnumerable<string> Post(PostData data)
        {
            yield return "<div class=\"post\">";
                yield return Include(PostHeader, data);
                yield return "<div class=\"content\">";
                    yield return data.Content;
                yield return "</div>";
            yield return "</div>";
        }

        
        public static IEnumerable<string> PostHeader(PostData data)
        {
            yield return $"<h2><a href=\"{data.Url}\">{HtmlEncode(data.Title)}</a></h2>";   

            yield return "<div class=\"meta\">";
                yield return $"<span class=\"date\"><span class=\"fas fa-calendar-alt\"></span>{data.Date}</span>";
                yield return $"<span class=\"category\"><span class=\"fas fa-folder\"></span>{HtmlEncode(data.Category)}</span>";
                var tags = data.Tags.Select(x => HtmlEncode(x));
                yield return $"<span class=\"tags\"><span class=\"fas fa-tags\"></span>{string.Join(", ", tags)}</span>";
            yield return "</div>";

            if (!string.IsNullOrEmpty(data.Subtitle))
                yield return $"<h3>{HtmlEncode(data.Subtitle)}</h3>";
        }

        public static IEnumerable<string> Page(PageData data)
        {
            yield return
$@"<!doctype html>
<html lang=""en"">
    <head>
        <meta charset=""utf-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0, maximum-scale=1"">
        <meta name=""description"" content="""">
        <meta name=""author"" content=""Zachary Snow"">

        <title>{data.Title}</title>
        <base href=""{data.BaseUrl}"">

        <link rel=""stylesheet"" href=""http://fonts.googleapis.com/css?family=PT+Serif:400,400italic,700|PT+Sans:400"">
        <link rel=""stylesheet"" type=""text/css"" href=""css/site.css"">

        <link rel=""alternate"" type=""application/rss+xml"" title=""The Blog of Zachary Snow"" href=""feed.rss"">
    </head>
    <body>
        <input type=""checkbox"" class=""sidebar-checkbox"" id=""sidebar-checkbox"" />
        <div class=""sidebar"" id=""sidebar"">
            <div class=""sidebar-item"">

            </div>
            <nav class=""sidebar-nav"">
                <a href=""index.html"" class=""sidebar-nav-item{If(data.FileName == "index.html", " active")}"">Home</a>
                <a href=""about.html"" class=""sidebar-nav-item{If(data.FileName == "about.html", " active")}"">About</a>
            </nav>
        </div>
        <div class=""wrap"">
            <div class=""masthead"">
                <div class=""container"">
                    <div class=""social"">
                        <a href=""http://twitter.com/smack0007"" class=""twitter"" title=""Twitter""><span class=""fab fa-twitter""></span></a>
                        <a href=""http://github.com/smack0007"" class=""github"" title=""Github""><span class=""fab fa-github""></span></a>
                        <a href=""feed.rss"" class=""rss"" title=""RSS""><span class=""fas fa-rss""></span></a>
                    </div>
                    <h3 class=""masthead-title"">
                        <a href=""index.html"" title=""Home"">The Blog of Zachary Snow</a>
                    </h3>
                </div>
            </div>
            <div class=""container content"">
                <div class=""posts"">
                    {data.Body}
                </div>
                <div class=""clear""></div>";
                
            if (data.ShowPagination)
            {
                yield return
@"              <div class=""pagination"">";

                if (data.PaginationOlderLink != null)
                {
                    yield return
$@"                  <a href=""{data.PaginationOlderLink}"" class=""pagination-item older"">Older</a>";                    
                }
                else
                {                    
                    yield return
@"                  <span class=""pagination-item older"">Older</span>";
                }
                    
                if (data.PaginationNewerLink != null)
                {
                    yield return
$@"                  <a href=""{data.PaginationNewerLink}"" class=""pagination-item newer"">Newer</a>";                    
                }
                else
                {
                    yield return     
@"                  <span class=""pagination-item newer"">Newer</span>";
                }

                yield return 
@"              </div>";
            }
            
            yield return
@"          </div>
        </div>
        <label for=""sidebar-checkbox"" class=""sidebar-toggle""></label>
    </body>
</html>";
        }

        public static IEnumerable<string> Index(IndexData data)
        {
            foreach (var post in data.Posts)
            {
                yield return "<div class=\"post\">";
                    yield return Include(Templates.PostHeader, post);
                    yield return "<div class=\"content\">";
                        yield return post.Excerpt;
                    yield return "</div>";
                yield return "</div>";
            }
        }
    }
}
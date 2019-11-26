using System.Collections.Generic;
using System.Linq;
using static compiler.TemplateExtensions;

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

            if (!string.IsNullOrEmpty(data.Subtitle))
                yield return $"<h3>{HtmlEncode(data.Subtitle)}</h3>";

            yield return "<div class=\"meta\">";
                yield return $"<span class=\"date\"><span class=\"icon-calendar\"></span>{data.Date}</span>";
                yield return $"<span class=\"category\"><span class=\"icon-folder\"></span>{HtmlEncode(data.Category)}</span>";
                var tags = data.Tags.Select(x => HtmlEncode(x));
                yield return $"<span class=\"tags\"><span class=\"icon-price-tags\"></span>{string.Join(", ", tags)}</span>";
            yield return "</div>";
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

        <link href=""https://fonts.googleapis.com/css?family=Open+Sans"" rel=""stylesheet"">
        <link rel=""stylesheet"" type=""text/css"" href=""css/style.css"">

        <link rel=""alternate"" type=""application/rss+xml"" title=""The Blog of Zachary Snow"" href=""feed.rss"">
    </head>
    <body>
        <div class=""wrap"">
            <nav class=""navbar navbar-expand-lg navbar-dark bg-dark"">
                <h1><a class=""navbar-brand"" href=""/"">The Blog of Zachary Snow</a></h1>
                <button id=""navbar-toggler"" class=""navbar-toggler"" type=""button"" aria-controls=""navbarSupportedContent"" aria-expanded=""false"" aria-label=""Toggle navigation"">
                    <span class=""navbar-toggler-icon""></span>
                </button>

                <div class=""collapse navbar-collapse"" id=""navbarSupportedContent"">
                    <ul class=""navbar-nav mr-auto"">                        
                        <li class=""nav-item{If(data.Url.EndsWith("index.html"), " active")}"">
                            <a class=""nav-link"" href=""index.html"">Home{If(data.Url.EndsWith("index.html"), " <span class=\"sr-only\">(current)</span>")}</a>
                        </li>
                        <li class=""nav-item{If(data.Url.EndsWith("about.html"), " active")}"">
                            <a class=""nav-link"" href=""about.html"">About{If(data.Url.EndsWith("about.html"), " <span class=\"sr-only\">(current)</span>")}</a>
                        </li>
                    </ul>
                    <div class=""social my-2 my-lg-0"">
                        <a href=""https://twitter.com/smack0007"" class=""twitter"" title=""Twitter""><span class=""icon-twitter""></span></a>
                        <a href=""https://github.com/smack0007"" class=""github"" title=""Github""><span class=""icon-github""></span></a>
                        <a href=""https://paypal.me/smack0007"" class=""coffee"" title=""Buy Me a Coffee""><span class=""icon-mug""></span></a>
                        <a href=""feed.rss"" class=""rss"" title=""RSS""><span class=""icon-rss""></span></a>
                    </ul>
                </div>
            </nav>
            <main class=""container"">
                <div class=""posts"">
                    {data.Body}
                </div>
                <div class=""clear""></div>";
                
            if (data.ShowPagination)
            {
                yield return
$@"              <nav aria-label=""Page navigation"">
                    <ul class=""pagination justify-content-center"">
                        <li class=""page-item{If(data.PaginationOlderLink == null, " disabled")}"">";

                if (data.PaginationOlderLink != null)
                {
                    yield return
$@"                         <a href=""{data.PaginationOlderLink}"" class=""page-link older"">Older</a>";                    
                }
                else
                {                    
                    yield return
@"                          <a class=""page-link older"" href=""#"" tabindex=""-1"">Older</a>";
                }

                yield return
$@"                      </li>
                        <li class=""page-item{If(data.PaginationNewerLink == null, " disabled")}"">";
                    
                if (data.PaginationNewerLink != null)
                {
                    yield return
$@"                         <a href=""{data.PaginationNewerLink}"" class=""page-link newer"">Newer</a>";                    
                }
                else
                {
                    yield return     
@"                          <a class=""page-link newer"" href=""#"" tabindex=""-1"">Newer</a>";
                }

                yield return 
@"                      </li>              
                    </ul>
                </div>";
            }
            
            yield return
@"          </main>
        </div>
        <script type=""text/javascript"">
            document.getElementById('navbar-toggler').onclick = function() {
                document.getElementById('navbarSupportedContent').classList.toggle('collapse');
            };
        </script>
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

                        if (post.HasExcerpt)
                            yield return $"<a class=\"readMore\" href=\"{post.Url}\">Read More</a>";

                    yield return "</div>";
                yield return "</div>";
            }
        }
    }
}
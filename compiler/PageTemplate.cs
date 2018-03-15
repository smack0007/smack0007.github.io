using System;
using System.Collections.Generic;

namespace compiler
{
    public class PageTemplate : Template<PageData>
    {
        protected override IEnumerable<string> RenderTemplate(PageData data)
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
        <link rel=""stylesheet"" href=""http://fonts.googleapis.com/css?family=PT+Serif:400,400italic,700|PT+Sans:400"">
        <link rel=""stylesheet"" type=""text/css"" href=""{data.BasePath}css/site.css"">

        <link rel=""alternate"" type=""application/rss+xml"" title=""The Blog of Zachary Snow"" href=""{data.BasePath}feed.rss"">
    </head>
    <body>
        <input type=""checkbox"" class=""sidebar-checkbox"" id=""sidebar-checkbox"" />
        <div class=""sidebar"" id=""sidebar"">
            <div class=""sidebar-item"">

            </div>
            <nav class=""sidebar-nav"">
                <a href=""{data.BasePath}index.html"" class=""sidebar-nav-item{If(data.FileName == "index.html", " active")}"">Home</a>
                <a href=""{data.BasePath}about.html"" class=""sidebar-nav-item{If(data.FileName == "about.html", " active")}"">About</a>
            </nav>
        </div>
        <div class=""wrap"">
            <div class=""masthead"">
                <div class=""container"">
                    <div class=""social"">
                        <a href=""http://twitter.com/smack0007"" class=""twitter"" title=""Twitter""><span class=""fab fa-twitter""></span></a>
                        <a href=""http://github.com/smack0007"" class=""github"" title=""Github""><span class=""fab fa-github""></span></a>
                        <a href=""{data.BasePath}feed.rss"" class=""rss"" title=""RSS""><span class=""fas fa-rss""></span></a>
                    </div>
                    <h3 class=""masthead-title"">
                        <a href=""{data.BasePath}index.html"" title=""Home"">The Blog of Zachary Snow</a>
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
$@"                  <a href=""{data.BasePath}{data.PaginationOlderLink}"" class=""pagination-item older"">Older</a>";                    
                }
                else
                {                    
                    yield return
@"                  <span class=""pagination-item older"">Older</span>";
                }
                    
                if (data.PaginationNewerLink != null)
                {
                    yield return
$@"                  <a href=""{data.BasePath}{data.PaginationNewerLink}"" class=""pagination-item newer"">Newer</a>";                    
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
    }
}
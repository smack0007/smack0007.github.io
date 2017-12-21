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
        <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
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
                <a href=""{data.BasePath}"" class=""sidebar-nav-item{If(data.FileName == "index.html", " active")}"">Home</a>
                <a href=""{data.BasePath}about.html"" class=""sidebar-nav-item{If(data.FileName == "about.html", " active")}"">About</a>
            </nav>
        </div>
        <div class=""wrap"">
            <div class=""masthead"">
                <div class=""container"">
                    <div class=""social"">
                        <a href=""http://twitter.com/smack0007"" class=""twitter"" title=""Twitter""><i class=""fa fa-twitter""></i></a>
                        <a href=""http://github.com/smack0007"" class=""github"" title=""Github""><i class=""fa fa-github""></i></a>
                        <a href=""{data.BasePath}feed.rss"" class=""rss"" title=""RSS""><i class=""fa fa-rss""></i></a>
                    </div>
                    <h3 class=""masthead-title"">
                        <a href=""{data.BasePath}"" title=""Home"">The Blog of Zachary Snow</a>
                    </h3>
                </div>
            </div>
            <div class=""container content"">
                <div class=""posts"">
                    {data.Body}
                </div>
                <div class=""pagination"">
                    <span class=""pagination-item older"">Older</span>
                    <span class=""pagination-item newer"">Newer</span>
                </div>
            </div>
        </div>
        <label for=""sidebar-checkbox"" class=""sidebar-toggle""></label>
    </body>
</html>";
        }
    }
}
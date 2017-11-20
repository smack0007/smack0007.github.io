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
        <link href=""https://fonts.googleapis.com/css?family=Open+Sans"" rel=""stylesheet"">
        <link rel=""stylesheet"" type=""text/css"" href=""{data.BasePath}css/site.css"">

        <link rel=""alternate"" type=""application/rss+xml"" title=""The Blog of Zachary Snow"" href=""{data.BasePath}feed.rss"">
    </head>
    <body>
        <header>
            <nav>
                <div class=""container"">
                    <a href=""{data.BasePath}index.html"" {If(data.FileName == "index.html", "class=\"active\"")}>Home</a>
                    <a href=""{data.BasePath}about.html"" {If(data.FileName == "about.html", "class=\"active\"")}>About</a>
                </div>
            </nav>
            <div class=""container"">
                <h1>The Blog of Zachary Snow</h1>
            </div>
        </header>
        <main class=""container"">
            <div class=""posts"">
                {data.Body}
            </div>
            <aside class=""sidebar"">
				<div class=""social"">
					<a href=""http://twitter.com/smack0007"" class=""twitter"" title=""Twitter""><i class=""fa fa-twitter""></i></a>
					<a href=""http://github.com/smack0007"" class=""github"" title=""Github""><i class=""fa fa-github""></i></a>
					<a href=""{data.BasePath}feed.rss"" class=""rss"" title=""RSS""><i class=""fa fa-rss""></i></a>
				</div>
            </aside>
        </main>
        <footer class=""container"">
        </footer>
    </body>
</html>";
        }
    }
}
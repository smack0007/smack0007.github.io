using System;
using System.Collections.Generic;

namespace compiler
{
    public class SiteTemplate : Template<SiteData>
    {
        protected override IEnumerable<string> RenderTemplate(SiteData data)
        {
            yield return $@"
<!doctype html>
<html lang=""en"">
    <head>
        <meta charset=""utf-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
        <meta name=""description"" content="""">
        <meta name=""author"" content=""Zachary Snow"">

        <title>{data.Title}</title>
        <link href=""https://fonts.googleapis.com/css?family=Open+Sans"" rel=""stylesheet"">
        <link rel=""stylesheet"" type=""text/css"" href=""{data.BasePath}css/site.css"">
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
        <main>
            <div class=""container"">
                {data.Body}
            </div>
        </main>
        <footer>
            <div class=""container"">
            </div>
        </footer>
    </body>
</html>";
        }
    }
}
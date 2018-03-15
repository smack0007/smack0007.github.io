using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using NUglify;
using NUglify.Css;
using NUglify.Html;

namespace compiler
{
    public static class Program
    {
        public const int PostsPerPage = 5;

        private static IndexTemplate IndexTemplate = new IndexTemplate();

        private static PostTemplate PostTemplate = new PostTemplate();

        private static PageTemplate PageTemplate = new PageTemplate();

        private static List<PageData> Pages = new List<PageData>();

        public static readonly CssSettings CssSettings = new CssSettings()
        {
            CommentMode = CssComment.None
        };

        public static readonly HtmlSettings HtmlSettings = new HtmlSettings()
        {
            CollapseWhitespaces = true,
            PrettyPrint = false,
        };

        public static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Please provide the root directory of the site and the output path.");
                return 1;
            }

            var inputPath = Path.GetFullPath(args[0]);
            var outputPath = Path.GetFullPath(args[1]);
            
            Directory.CreateDirectory(outputPath);
            
            Console.WriteLine($"Compiling path: {inputPath}");

            var files = Directory.EnumerateFiles(inputPath, "*.*", SearchOption.AllDirectories).Select(x => x.Substring(inputPath.Length)).ToArray();
            var markdownFiles = files.Where(x => Path.GetExtension(x) == ".md");
            var cssFiles = files.Where(x => Path.GetExtension(x) == ".css");
            var staticFiles = files.Except(markdownFiles).Except(cssFiles);

            var posts = new List<PostData>();

            foreach (var fileName in markdownFiles)
            {
                var outputDirectory = Path.Combine(outputPath, Path.GetDirectoryName(fileName));
                var outputFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileName) + ".html");
                
                var isPost = fileName.StartsWith("blog" + Path.DirectorySeparatorChar);
                Console.WriteLine((isPost ? "Post" : "Page") + $": {fileName} => {outputFileName}");

                Directory.CreateDirectory(outputDirectory);

                var content = ProcessMarkdown(
                    File.ReadAllLines(Path.Combine(inputPath, fileName)),
                    out var frontMatter);

                if (isPost)
                {
                    var postData = new PostData(
                        frontMatter,
                        content,
                        Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".html"));

                    posts.Add(postData);

                    RenderPost(
                        fileName,
                        outputFileName,
                        postData);
                }
                else
                {
                    RenderPage(
                        fileName,
                        outputFileName,
                        frontMatter["Title"],
                        content,
                        showPagination: false);
                }
            }

            var sb = new StringBuilder(1024);
            foreach (var cssFile in cssFiles)
            {
                Console.WriteLine($"Css: {cssFile}");
                
                var css = File.ReadAllText(Path.Combine(inputPath, cssFile));

                sb.Append(css);                
            }
            
            Directory.CreateDirectory(Path.Combine(outputPath, "css"));
            File.WriteAllText(Path.Combine(outputPath, "css", "site.css"), Uglify.Css(sb.ToString(), CssSettings).Code);

            foreach (var staticFile in staticFiles)
            {
                Console.WriteLine($"Static: {staticFile}");

                var output = Path.Combine(outputPath, staticFile);

                Directory.CreateDirectory(Path.GetDirectoryName(output));

                File.Copy(
                    Path.Combine(inputPath, staticFile),
                    output,
                    true);
            }
            
            int pageCount = (int)Math.Ceiling(posts.Count / (double)PostsPerPage);
            Console.WriteLine($"{posts.Count} Posts / {pageCount} Pages");

            for (int i = 0; i < pageCount; i++)
            {
                string GetPageName(int index)
                {
                    if (index < 0 || index >= pageCount)
                    {
                        return null;
                    }

                    return index == 0 ? "index.html" : Path.Combine("blog", "page" + (index + 1) + ".html");
                }

                var pageName = GetPageName(i);
                Console.WriteLine($"Index: {pageName}");

                RenderPage(
                    pageName,
                    Path.Combine(outputPath, GetPageName(i)),
                    "The Blog of Zachary Snow",
                    IndexTemplate.Render(new IndexData(posts.OrderByDescending(x => x.SortDate).Skip(i * PostsPerPage).Take(PostsPerPage))),
                    showPagination: true,
                    paginationOlderLink: GetPageName(i + 1),
                    paginationNewerLink: GetPageName(i - 1));
            }

            File.WriteAllText(Path.Combine(outputPath, "feed.rss"), GenerateRssFeed(posts));
            File.WriteAllText(Path.Combine(outputPath, "sitemap.xml"), GenerateSiteMap(Pages));

            return 0;
        }

        private static string ProcessMarkdown(
            string[] lines,
            out FrontMatter frontMatter)
        {
            lines = Utility.TrimEmptyLines(lines);

            string[] frontMatterLines;
            string[] contentLines;
            ExtractMarkdownParts(lines, out frontMatterLines, out contentLines);

            frontMatter = FrontMatter.Parse(frontMatterLines);
            return Markdown.ToHtml(ReplaceVariables(string.Join(Environment.NewLine, contentLines)));
        }

        private static void ExtractMarkdownParts(string[] lines, out string[] frontMatter, out string[] contentLines)
        {
            frontMatter = null;
            contentLines = null;

            int markdownStart = 0;

            if (lines[0].Trim() == "---")
            {
                int frontMatterEnd = 0;

                frontMatterEnd++;
                while (frontMatterEnd < lines.Length && lines[frontMatterEnd].Trim() != "---")
                {
                    frontMatterEnd++;
                }

                if (frontMatterEnd != lines.Length)
                {
                    contentLines = new string[frontMatterEnd - 1];
                    Array.Copy(lines, 1, contentLines, 0, frontMatterEnd - 1);
                }

                markdownStart = frontMatterEnd + 1;
            }

            if (markdownStart != 0)
            {
                frontMatter = new string[markdownStart - 2];
                Array.Copy(lines, 1, frontMatter, 0, frontMatter.Length);
            }

            while (lines[markdownStart].Trim().Length == 0)
                markdownStart++;

            contentLines = new string[lines.Length - markdownStart];
            Array.Copy(lines, markdownStart, contentLines, 0, lines.Length - markdownStart);
        }

        private static string ReplaceVariables(string input)
        {
            return Regex.Replace(input, "\\{\\{.*\\}\\}", (m) => {
                switch (m.ToString())
                {
                    case "{{baseUrl}}": return "http://smack0007.github.io";
                }

                return m.ToString();
            });
        }

        public static void RenderPost(
            string fileName,
            string outputFileName,
            PostData postData)
        {            
            RenderPage(fileName, outputFileName, postData.Title, PostTemplate.Render(postData));
        }

        private static void RenderPage(
            string fileName,
            string outputFile,
            string title,
            string body,
            bool showPagination = true,
            string paginationOlderLink = null,
            string paginationNewerLink = null)
        {
            string basePath = string.Empty;
            string[] pathParts = fileName.Split(new char[] { '/', '\\' });
            
            for (int i = 0; i < pathParts.Length - 1; i++)
                basePath += "../";

            var page = new PageData(fileName, basePath, title, body, showPagination, paginationOlderLink, paginationNewerLink);
            Pages.Add(page);

            File.WriteAllText(
                outputFile,
                Uglify.Html(PageTemplate.Render(page), HtmlSettings).Code);
        }

        private static string GenerateRssFeed(List<PostData> posts)
        {
            var sb = new StringBuilder(1024);
            
            var pubDate = (DateTime)posts.Select(x => x.SortDate).Max();		

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("\t<channel>");
            sb.AppendLine("\t<title>The Blog of Zachary Snow</title>");
            sb.AppendLine("\t<description>The Blog of Zachary Snow</description>");
            sb.AppendLine("\t<link>http://smack0007.github.io/</link>");
            sb.AppendLine("\t<lastBuildDate>" + DateTime.Now.ToString("r") + "</lastBuildDate>");
            sb.AppendLine("\t<pubDate>" + pubDate.ToString("r") + "</pubDate>");
            sb.AppendLine("\t<ttl>1800</ttl>");
            
            foreach (var post in posts.OrderByDescending(x => x.SortDate).Take(20))
            {	
                sb.AppendLine("\t<item>");
                sb.AppendLine("\t\t<title><![CDATA[" + post.Title + "]]></title>");
                sb.AppendLine("\t\t<description><![CDATA[" + post.Excerpt + "]]></description>");
                sb.AppendLine("\t\t<link>http://smack0007.github.io/" + post.Url + "</link>");
                sb.AppendLine("\t\t<guid>http://smack0007.github.io/" + post.Url + "</guid>");
                sb.AppendLine("\t\t<pubDate>" + post.SortDate.ToString("r") + "</pubDate>");
                sb.AppendLine("\t</item>");
            }

            sb.AppendLine("\t</channel>");
            sb.AppendLine("</rss>");
            sb.AppendLine();
            
            return sb.ToString();
        }

        private static string GenerateSiteMap(List<PageData> pages)
        {
            var sb = new StringBuilder(1024);
        
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                    
            sb.AppendLine("\t<url>");
            sb.AppendLine("\t\t<loc>http://smack0007.github.io/index.html</loc>");
            sb.AppendLine("\t</url>");

            foreach (var page in pages)
            {		
                sb.AppendLine("\t<url>");
                sb.AppendLine("\t\t<loc>http://smack0007.github.io/" + page.Url + "</loc>");
                sb.AppendLine("\t</url>");
            }
        
            // foreach (var page in site.Pages)
            // {		
            //     sb.AppendLine("\t<url>");
            //     sb.AppendLine("\t\t<loc>http://smack0007.github.io/" + page.Permalink + "</loc>");
            //     sb.AppendLine("\t</url>");
            // }
            
            // foreach (var page in site.GeneratorPages.Where(x => x.Data.IsRedirect != true))
            // {		
            //     sb.AppendLine("\t<url>");
            //     sb.AppendLine("\t\t<loc>http://smack0007.github.io/" + page.Permalink + "</loc>");
            //     sb.AppendLine("\t</url>");
            // }
        
            sb.AppendLine("</urlset>");
            sb.AppendLine();
            
            return sb.ToString();
        }
    }
}

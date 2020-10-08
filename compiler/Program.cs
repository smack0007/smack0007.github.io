using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using NUglify;
using NUglify.Css;
using NUglify.Html;
using ColorCode;
using System.Net;
using SharpScss;
using HtmlSyntaxHighlighterDotNet;
using System.Threading.Tasks;
using TextDecoratorDotNet;

namespace compiler
{
    public static class Program
    {
        public const string BlogTitle = "The Blog of Zachary Snow";

        public const string BaseUrl = "http://smack0007.github.io";

        public const int PostsPerPage = 5;

        private static List<PageData> Pages = new List<PageData>();

        private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        private static readonly CssSettings UglifyCssSettings = new CssSettings()
        {
            CommentMode = CssComment.None
        };

        private static readonly HtmlSettings UglifyHtmlSettings = new HtmlSettings()
        {
            CollapseWhitespaces = true,
            PrettyPrint = false,
        };

        private static Template<PostData>? PostHeaderTemplate;        
        private static Template<PostData>? PostTemplate;
        private static Template<PageData>? PageTemplate;
        private static Template<IndexData>? IndexTemplate;
        private static Template<TagsData>? TagsTemplate;

        public async static Task<int> Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Please provide the root directory of the site and the output path.");
                return 1;
            }

            var inputPath = Path.GetFullPath(args[0]);
            var outputPath = Path.GetFullPath(args[1]);
            
            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(Path.Combine(outputPath, "blog"));
            Directory.CreateDirectory(Path.Combine(outputPath, "tags"));
            
            Console.WriteLine($"Input path: {inputPath}");

            var files = Directory.EnumerateFiles(inputPath, "*.*", SearchOption.AllDirectories)
                .Select(x => x.Substring(inputPath.Length))
                .ToArray();

            var templateFiles = files.Where(x => Path.GetExtension(x) == ".cshtml");
            var markdownFiles = files.Where(x => Path.GetExtension(x) == ".md");
            var cssFiles = files.Where(x => Path.GetExtension(x) == ".css" || Path.GetExtension(x) == ".scss");
            
            var staticFiles = files
                .Except(templateFiles)
                .Except(markdownFiles)
                .Except(cssFiles);

            var templatesDirectory = Path.Combine(inputPath, "templates");
            PostHeaderTemplate = await Template.CompileAsync<PostData>(File.ReadAllText(Path.Combine(templatesDirectory, "postHeader.cshtml")));
            PostTemplate = await Template.CompileAsync<PostData>(File.ReadAllText(Path.Combine(templatesDirectory, "post.cshtml")));
            PageTemplate = await Template.CompileAsync<PageData>(File.ReadAllText(Path.Combine(templatesDirectory, "page.cshtml")));
            IndexTemplate = await Template.CompileAsync<IndexData>(File.ReadAllText(Path.Combine(templatesDirectory, "index.cshtml")));
            TagsTemplate = await Template.CompileAsync<TagsData>(File.ReadAllText(Path.Combine(templatesDirectory, "tags.cshtml")));

            var posts = new List<PostData>();

            foreach (var fileName in markdownFiles)
            {
                var fileDirectory = Path.GetDirectoryName(fileName);
                var outputDirectory = fileDirectory != null ? Path.Combine(outputPath, fileDirectory) : outputPath;
                var outputFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileName) + ".html");
                
                var isPost = fileName.StartsWith("blog" + Path.DirectorySeparatorChar);

                Directory.CreateDirectory(outputDirectory);

                (var frontMatter, var content) = ProcessMarkdown(await File.ReadAllLinesAsync(Path.Combine(inputPath, fileName)));

                if (isPost)
                {
                    string path = Path.GetFileNameWithoutExtension(fileName) + ".html";
                    
                    if (fileDirectory != null)
                        path = Path.Combine(fileDirectory, path);

                    var postData = new PostData(
                        fileName,
                        outputFileName,
                        frontMatter,
                        content,
                        path,
                        x => PostHeaderTemplate.Run(x));

                    posts.Add(postData);
                }
                else
                {
                    Console.WriteLine($"Page: {fileName} => {outputFileName}");

                    RenderPage(
                        fileName,
                        outputFileName,
                        frontMatter["Title"],
                        content,
                        showPagination: false);
                }
            }

            posts = posts.OrderByDescending(x => x.SortDate).ToList();

            for (int i = 0; i < posts.Count; i++)
            {
                if (i > 0)
                    posts[i].NewerPost = posts[i - 1];

                if (i < posts.Count - 1)
                    posts[i].OlderPost = posts[i + 1];
            }

            if (PostTemplate == null)
                throw new InvalidOperationException($"{nameof(PostTemplate)} is null.");

            foreach (var post in posts)
            {
                Console.WriteLine($"Post: {post.FileName} => {post.OutputFileName}");

                RenderPage(
                    post.FileName,
                    post.OutputFileName,
                    post.Title,
                    PostTemplate.Run(post),
                    showPagination: true,
                    post.OlderPost?.Url,
                    post.NewerPost?.Url);
            }

            var cssInputPath = Path.Combine(inputPath, "css");

            var scssOptions = new ScssOptions()
            {
                TryImport = (string file, string path, out string? scss, out string? map) =>
                {
                    scss = null;
                    map = null;

                    var importPath = Path.Combine(cssInputPath, file);

                    if (!File.Exists(importPath))
                    {
                        Console.Error.WriteLine($"ERROR: Failed to include '{file}'.");
                        return false;
                    }

                    scss = File.ReadAllText(importPath);
                    return true;
                }
            };

            Directory.CreateDirectory(Path.Combine(outputPath, "css"));

            File.WriteAllText(Path.Combine(cssInputPath, "HtmlSyntaxHighlighterDotNet.css"), "/* This file is auto generated. */" + Environment.NewLine + HtmlSyntaxHighlighter.GetCssStyles());

            var cssResult = Scss.ConvertToCss(File.ReadAllText(Path.Combine(cssInputPath, "style.scss")), scssOptions);
            File.WriteAllText(Path.Combine(outputPath, "css", "style.css"), Uglify.Css(cssResult.Css, UglifyCssSettings).Code);

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

            if (IndexTemplate == null)
                throw new InvalidOperationException($"{nameof(IndexTemplate)} is null.");

            for (int i = 0; i < pageCount; i++)
            {
                string? GetIndexPageName(int index)
                {
                    if (index < 0 || index >= pageCount)
                        return null;

                    return index == 0 ? "index.html" : Path.Combine("blog", "page" + (index + 1) + ".html");
                }

                var pageName = GetIndexPageName(i) ?? "";
                Console.WriteLine($"Index: {pageName}");

                var indexPosts = posts.OrderByDescending(x => x.SortDate).Skip(i * PostsPerPage).Take(PostsPerPage);

                RenderPage(
                    pageName,
                    Path.Combine(outputPath, pageName),
                    BlogTitle,
                    IndexTemplate.Run(new IndexData(indexPosts, x => PostHeaderTemplate.Run(x))),
                    showPagination: true,
                    paginationOlderLink: GetIndexPageName(i + 1),
                    paginationNewerLink: GetIndexPageName(i - 1));
            }

            var tagNames = posts.SelectMany(x => x.Tags).Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(x => x);
            var tags = new List<TagData>();
            
            foreach (var tagName in tagNames)
            {
                var tagPath = Path.Combine("tags", $"{Utility.InflectFileName(tagName)}.html");

                var tagPosts = posts
                    .Where(x => x.Tags.Contains(tagName, StringComparer.InvariantCultureIgnoreCase))
                    .OrderByDescending(x => x.SortDate)
                    .ToArray();

                Console.WriteLine($"Tag: ({tagPosts.Length}) {tagPath}");

                RenderPage(
                    tagPath,
                    Path.Combine(outputPath, tagPath),
                    tagName,
                    IndexTemplate.Run(new IndexData(tagPosts, x => PostHeaderTemplate.Run(x))),
                    showPagination: false);

                tags.Add(new TagData(tagName, tagPath, tagPosts));
            }

            RenderPage(
                "tags.html",
                Path.Combine(outputPath, "tags.html"),
                "Tags",
                TagsTemplate.Run(new TagsData(tags)),
                showPagination: false);

            File.WriteAllText(Path.Combine(outputPath, "feed.rss"), GenerateRssFeed(posts));
            File.WriteAllText(Path.Combine(outputPath, "sitemap.xml"), GenerateSiteMap(Pages));

            return 0;
        }

        private static (FrontMatter FrontMatter, string Content) ProcessMarkdown(string[] lines)
        {
            lines = Utility.TrimEmptyLines(lines);

            (var frontMatterLines, var contentLines) = ExtractMarkdownParts(lines);

            return (
                FrontMatter.Parse(frontMatterLines),
                PostprocessContent(Markdown.ToHtml(PreprocessContent(string.Join(Environment.NewLine, contentLines)), MarkdownPipeline))
            );
        }

        private static (string[] FrontMatter, string[] Content) ExtractMarkdownParts(string[] lines)
        {
            string[] frontMatter = Array.Empty<string>();
            string[] content = Array.Empty<string>();

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
                    content = new string[frontMatterEnd - 1];
                    Array.Copy(lines, 1, content, 0, frontMatterEnd - 1);
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

            content = new string[lines.Length - markdownStart];
            Array.Copy(lines, markdownStart, content, 0, lines.Length - markdownStart);

            return (frontMatter, content);
        }

        private static string PreprocessContent(string input)
        {
            input = ReplaceVariables(input);

            return input;
        }

        private static string PostprocessContent(string input)
        {
            input = AddSyntaxHighlighting(input);

            return input;
        }

        private static string ReplaceVariables(string input)
        {
            return Regex.Replace(input, "\\{\\{.*\\}\\}", (m) => {
                string match = m.ToString();

                switch (match)
                {
                    case "{{baseUrl}}": return BaseUrl;
                }

                return match;
            });
        }

        private static string AddSyntaxHighlighting(string input)
        {
            return Regex.Replace(input, "\\<pre\\>\\<code class\\=\\\"language\\-.*?\\<\\/code\\>\\<\\/pre\\>",  (m) => {
                string codeBlock = m.ToString();
                
                const string header = "<pre><code class=\"language-";
                const string footer = "</pre></code>";
                codeBlock = codeBlock.Substring(header.Length, codeBlock.Length - header.Length - footer.Length);

                const string separator = "\">";
                int separatorIndex = codeBlock.IndexOf(separator);
                string languageName = codeBlock.Substring(0, separatorIndex);
                codeBlock = codeBlock.Substring(separatorIndex + separator.Length);

                codeBlock = WebUtility.HtmlDecode(codeBlock);

                ILanguage language;

                switch (languageName.ToLower())
                {
                    case "bat":
                    case "cmd":
                        return m.ToString();

                    case "cpp":
                    case "c++":
                        language = Languages.Cpp;
                        break;

                    case "csharp":
                    case "c#":
                        language = Languages.CSharp;
                        break;

                    case "html":
                        language = Languages.Html;
                        break;

                    case "js":
                    case "json":
                    case "javascript":
                    case "ts":
                    case "typescript":
                        language = Languages.JavaScript;
                        break;

                    case "php":
                        language = Languages.Php;
                        break;

                    case "xml":
                        language = Languages.Xml;
                        break;

                    case "":
                        return m.ToString();

                    default:
                        Console.Error.WriteLine($"ERROR: Unknown language '{languageName}'.");
                        return m.ToString();
                }
                
                if (language == Languages.CSharp)
                {
                    codeBlock = HtmlSyntaxHighlighter.TransformCSharp(codeBlock);
                }
                else
                {
                    codeBlock = new HtmlClassFormatter().GetHtmlString(codeBlock, language);
                    
                    // Remove surrounding <div> and <pre> tags generated by ColorCode.
                    const string preOpeningTag = "<pre>";
                    const string preClosingTag = "</pre>";
                    int preOpeningTagIndex = codeBlock.IndexOf(preOpeningTag);
                    int preClosingTagIndex = codeBlock.LastIndexOf(preClosingTag);
                    codeBlock = codeBlock.Substring(preOpeningTagIndex + preOpeningTag.Length, preClosingTagIndex - preOpeningTagIndex - preOpeningTag.Length);
                }

                return header + languageName + separator + codeBlock.Trim() + footer;
            }, RegexOptions.Singleline);
        }

        private static void RenderPage(
            string fileName,
            string outputFile,
            string title,
            string body,
            bool showPagination = true,
            string? paginationOlderLink = null,
            string? paginationNewerLink = null)
        {
            if (PageTemplate == null)
                throw new InvalidOperationException($"{nameof(PageTemplate)} is null.");

            var page = new PageData(
                fileName,
                CalculateBaseUrl(fileName),
                title,
                body,
                showPagination,
                paginationOlderLink,
                paginationNewerLink);

            Pages.Add(page);

            File.WriteAllText(
                outputFile,
                Uglify.Html(PageTemplate.Run(page), UglifyHtmlSettings).Code);
        }

        private static string CalculateBaseUrl(string fileName)
        {
            string basePath = string.Empty;
            string[] pathParts = fileName.Split(new char[] { '/', '\\' });

            for (int i = 0; i < pathParts.Length - 1; i++)
                basePath += "../";
            
            return basePath;
        }

        private static string GenerateRssFeed(List<PostData> posts)
        {
            var sb = new StringBuilder(1024);
            
            var pubDate = (DateTime)posts.Select(x => x.SortDate).Max();		

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.AppendLine("<rss version=\"2.0\">");
            sb.AppendLine("\t<channel>");
            sb.AppendLine($"\t<title>{BlogTitle}</title>");
            sb.AppendLine($"\t<description>{BlogTitle}</description>");
            sb.AppendLine($"\t<link>{BaseUrl}/</link>");
            sb.AppendLine($"\t<lastBuildDate>{DateTime.Now.ToString("r")}</lastBuildDate>");
            sb.AppendLine($"\t<pubDate>{pubDate.ToString("r")}</pubDate>");
            sb.AppendLine("\t<ttl>1800</ttl>");
            
            foreach (var post in posts.OrderByDescending(x => x.SortDate).Take(20))
            {	
                sb.AppendLine("\t<item>");
                sb.AppendLine($"\t\t<title><![CDATA[{post.Title}]]></title>");
                sb.AppendLine($"\t\t<description><![CDATA[{post.Excerpt}]]></description>");
                sb.AppendLine($"\t\t<link>{BaseUrl}/{post.Url}</link>");
                sb.AppendLine($"\t\t<guid>{BaseUrl}/{post.Url}</guid>");
                sb.AppendLine($"\t\t<pubDate>{post.SortDate.ToString("r")}</pubDate>");
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
            sb.AppendLine($"\t\t<loc>{BaseUrl}/index.html</loc>");
            sb.AppendLine("\t</url>");

            foreach (var page in pages)
            {		
                sb.AppendLine("\t<url>");
                sb.AppendLine($"\t\t<loc>{BaseUrl}/{page.Url}</loc>");
                sb.AppendLine("\t</url>");
            }
        
            sb.AppendLine("</urlset>");
            sb.AppendLine();
            
            return sb.ToString();
        }
    }
}

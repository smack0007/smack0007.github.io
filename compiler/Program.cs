using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Fluid;
using Markdig;

namespace compiler
{
    public static class Program
    {
        private static FluidTemplate IndexTemplate;

        private static FluidTemplate PostTempate;

        private static FluidTemplate SiteTempate;

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

            IndexTemplate = ParseTemplate("index.liquid");
            PostTempate = ParseTemplate("post.liquid");
            SiteTempate = ParseTemplate("site.liquid");

            var files = Directory.EnumerateFiles(inputPath, "*.*", SearchOption.AllDirectories).Select(x => x.Substring(inputPath.Length)).ToArray();
            var markdownFiles = files.Where(x => Path.GetExtension(x) == ".md");
            var cssFiles = files.Where(x => Path.GetExtension(x) == ".css");
            var staticFiles = files.Except(markdownFiles).Except(cssFiles);

            var posts = new List<PostData>();

            foreach (var fileName in markdownFiles)
            {
                var outputDirectory = Path.Combine(outputPath, Path.GetDirectoryName(fileName));
                var outputFileName = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileName) + ".html");
                Console.WriteLine($"Post: {fileName} => {outputFileName}");

                Directory.CreateDirectory(outputDirectory);

                var content = ProcessMarkdown(
                    File.ReadAllLines(Path.Combine(inputPath, fileName)),
                    out var frontMatter);

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

            var sb = new StringBuilder(1024);
            foreach (var cssFile in cssFiles)
            {
                Console.WriteLine($"Css: {cssFile}");
                sb.Append(File.ReadAllText(Path.Combine(inputPath, cssFile)));                
            }
            Directory.CreateDirectory(Path.Combine(outputPath, "css"));
            File.WriteAllText(Path.Combine(outputPath, "css", "site.css"), sb.ToString());

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

            var context = new TemplateContext();
            context.MemberAccessStrategy.Register<IndexData>();
            context.MemberAccessStrategy.Register<PostData>();
            context.MemberAccessStrategy.Register<FrontMatter>((obj, name) => obj[name]);
            context.SetValue("Model", new IndexData(posts));
            
            RenderSitePage(
                Path.Combine(outputPath, "index.html"),
                string.Empty,
                "The Blog of Zachary Snow",
                IndexTemplate.Render(context));

            return 0;
        }

        private static FluidTemplate ParseTemplate(string fileName)
        {
            if (!FluidTemplate.TryParse(
                File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "layouts", fileName)),
                out var template,
                out var errors))
            {
                throw new InvalidOperationException($"Failed to parse tempalte '{fileName}': {string.Join(Environment.NewLine, errors)}");
            }

            return template;
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
            return Markdown.ToHtml(string.Join(Environment.NewLine, contentLines));
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

        public static void RenderPost(
            string fileName,
            string outputFileName,
            PostData postData)
        {
            var context = new TemplateContext();
            context.MemberAccessStrategy.Register<PostData>();
            context.MemberAccessStrategy.Register<FrontMatter>((obj, name) => obj[name]);
            context.SetValue("Model", postData);

            string basePath = string.Empty;
            string[] pathParts = fileName.Split(new char[] { '/', '\\' });
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                basePath += "../";
            }
            
            RenderSitePage(outputFileName, basePath, postData.FrontMatter["Title"], PostTempate.Render(context));
        }

        private static void RenderSitePage(
            string outputFile,
            string basePath,
            string title,
            string body)
        {
            var context = new TemplateContext();
            context.MemberAccessStrategy.Register<SiteData>();
            context.SetValue("Model", new SiteData(basePath, title, body));

            File.WriteAllText(outputFile, SiteTempate.Render(context));
        }
    }
}

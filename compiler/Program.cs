using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fluid;
using Markdig;

namespace compiler
{
    public static class Program
    {
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

            IEnumerable<string> errors;

            if (!FluidTemplate.TryParse(
                File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "layouts", "post.liquid")),
                out PostTempate,
                out errors))
            {
                throw new InvalidOperationException($"Failed to parse post.liquid: {string.Join(Environment.NewLine, errors)}");
            }

            if (!FluidTemplate.TryParse(
                File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "layouts", "site.liquid")),
                out SiteTempate,
                out errors))
            {
                throw new InvalidOperationException($"Failed to parse site.liquid: {string.Join(Environment.NewLine, errors)}");
            }

            var files = Directory.EnumerateFiles(inputPath, "*.*", SearchOption.AllDirectories).Select(x => x.Substring(inputPath.Length)).ToArray();
            var markdownFiles = files.Where(x => Path.GetExtension(x) == ".md");
            var staticFiles = files.Except(markdownFiles);

            foreach (var markdownFile in markdownFiles)
            {
                ProcessMarkdown(inputPath, markdownFile, outputPath);
            }

            foreach (var staticFile in staticFiles)
            {
                Console.WriteLine($"StaticFile: {staticFile}");

                File.Copy(
                    Path.Combine(inputPath, staticFile),
                    Path.Combine(outputPath, staticFile),
                    true);
            }

            return 0;
        }

        public static void ProcessMarkdown(string inputPath, string fileName, string outputPath)
        {
            var outputDirectory = Path.Combine(outputPath, Path.GetDirectoryName(fileName));
            var outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileName) + ".html");
            Console.WriteLine($"{nameof(ProcessMarkdown)}: {fileName} => {outputFile}");

            Directory.CreateDirectory(outputDirectory);

            var lines = Utility.TrimEmptyLines(File.ReadAllLines(Path.Combine(inputPath, fileName)));

            string[] frontMatterLines;
            string[] contentLines;
            ExtractMarkdownParts(lines, out frontMatterLines, out contentLines);

            var frontMatter = FrontMatter.Parse(frontMatterLines);
            var content = Markdown.ToHtml(string.Join(Environment.NewLine, contentLines));
               
            var context = new TemplateContext();
            context.MemberAccessStrategy.Register<PostData>();
            context.MemberAccessStrategy.Register<FrontMatter>((obj, name) => obj[name]);
            context.SetValue("Model", new PostData(frontMatter, content));

            string basePath = string.Empty;
            string[] pathParts = fileName.Split(new char[] { '/', '\\' });
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                basePath += "../";
            }
            
            RenderSitePage(outputFile, basePath, frontMatter["Title"], PostTempate.Render(context));
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

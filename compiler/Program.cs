﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdig;

namespace compiler
{
    public static class Program
    {
        private static IndexTemplate IndexTemplate = new IndexTemplate();

        private static PostTemplate PostTemplate = new PostTemplate();

        private static SiteTemplate SiteTempate = new SiteTemplate();

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
                
                var css = File.ReadAllText(Path.Combine(inputPath, cssFile));

                if (!cssFile.EndsWith(".min.css"))
                    css = $" /* {cssFile} */ " + Utility.RemoveWhiteSpaceFromCss(css);

                sb.Append(css);                
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
            
            RenderSitePage(
                "index.html",
                Path.Combine(outputPath, "index.html"),
                "The Blog of Zachary Snow",
                IndexTemplate.Render(new IndexData(posts)));

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
            RenderSitePage(fileName, outputFileName, postData.Title, PostTemplate.Render(postData));
        }

        private static void RenderSitePage(
            string fileName,
            string outputFile,
            string title,
            string body)
        {
            string basePath = string.Empty;
            string[] pathParts = fileName.Split(new char[] { '/', '\\' });
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                basePath += "../";
            }

            File.WriteAllText(
                outputFile,
                SiteTempate.Render(new SiteData(fileName, basePath, title, body)));
        }
    }
}

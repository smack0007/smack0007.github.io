using System;
using System.IO;
using Markdig;

namespace compiler
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Please provide the root directory of the site.");
                return 1;
            }

            var path = Path.GetFullPath(args[0]);

            Console.WriteLine($"Compiling path: {path}");

            var markdownFiles = Directory.EnumerateFiles(path, "*.md", SearchOption.AllDirectories);

            foreach (var markdownFile in markdownFiles)
            {
                ProcessMarkdown(markdownFile);
            }

            return 0;
        }

        public static void ProcessMarkdown(string fileName)
        {
            var outputFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".html");
            Console.WriteLine($"{nameof(ProcessMarkdown)}: {fileName} => {outputFile}");

            var lines = Utility.TrimEmptyLines(File.ReadAllLines(fileName));

            string[] frontMatterLines;
            string[] contentLines;
            ExtractMarkdownParts(lines, out frontMatterLines, out contentLines);

            var html = Markdown.ToHtml(string.Join(Environment.NewLine, contentLines));

            File.WriteAllText(outputFile, html);
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
    }
}

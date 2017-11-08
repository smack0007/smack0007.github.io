using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Fluid;
using Markdig;

namespace compiler
{
    public static class Program
    {
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

            var fileNames = Directory.EnumerateFiles(inputPath, "*.*", SearchOption.AllDirectories).Select(x => x.Substring(inputPath.Length));

            foreach (var markdownFileName in fileNames.Where(x => Path.GetExtension(x) == ".md"))
            {
                ProcessMarkdown(inputPath, markdownFileName, outputPath);
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

            var source = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "layouts", "post.liquid"));

            if (FluidTemplate.TryParse(source, out var template))
            {   
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register<MarkdownData>();
                context.MemberAccessStrategy.Register<FrontMatter>((obj, name) => obj[name]);
                context.SetValue("Model", new MarkdownData(frontMatter, content));

                File.WriteAllText(outputFile, template.Render(context));
            }
            else
            {
                throw new InvalidOperationException("Failed to parse post.liquid");
            }
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

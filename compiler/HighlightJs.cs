using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace compiler
{
    public static class HighlightJs
    {
        public static string Highlight(ref string language, string input)
        {
            language = InflectLanguageName(language);

            using var process = new Process();
            process.StartInfo.FileName = "node";
            process.StartInfo.Arguments = $"highlight.js {language}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();

            using (var sw = process.StandardInput)
                sw.WriteLine(input);

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }

        public static string InflectLanguageName(string language)
        {
            switch (language.ToLower())
            {
                case "bat":
                case "cmd":
                case "shell":
                    return "shell";

                case "cpp":
                case "c++":
                    return "cpp";

                case "csharp":
                case "c#":
                    return "csharp";

                case "html":
                    return "html";

                case "js":
                case "javascript":
                    return "javascript";
                    
                case "json":    
                    return "json";

                case "php":
                    return "php";

                case "ts":
                case "typescript":
                    return "typescript";

                case "xml":
                    return "xml";

                case "yaml":
                case "yml":
                    return "yaml";
            }

            Console.Error.WriteLine($"ERROR: Unknown language '{language}'.");
            return language;
        }
    }
}
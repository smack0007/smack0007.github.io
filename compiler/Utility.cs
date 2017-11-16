using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace compiler
{
    public static class Utility
    {
        public static string[] TrimEmptyLines(string[] lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            if (lines.Length == 0)
                return lines;

            int start = 0;
            int end = lines.Length - 1;

            while (lines[start].Trim() == string.Empty)
            {
                start++;
            }

            while (lines[end].Trim() == string.Empty)
            {
                end--;
            }

            if (start == 0 && end == lines.Length - 1)
            {
                return lines;
            }

            var newLines = new string[end - start + 1];
            Array.Copy(lines, start, newLines, 0, end - start + 1);
            return newLines;
        }

        // Taken from https://madskristensen.net/blog/efficient-stylesheet-minification-in-c/
        public static string RemoveWhiteSpaceFromCss(string body)
        {
            body = Regex.Replace(body, @"[a-zA-Z]+#", "#");
            body = Regex.Replace(body, @"[\n\r]+\s*", string.Empty);
            body = Regex.Replace(body, @"\s+", " ");
            body = Regex.Replace(body, @"\s?([:,;{}])\s?", "$1");
            body = body.Replace(";}", "}");
            body = Regex.Replace(body, @"([\s:]0)(px|pt|%|em)", "$1");
            
            // Remove comments from CSS
            body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty);
            
            return body;
        }
    }
}

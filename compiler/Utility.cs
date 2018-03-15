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
    }
}

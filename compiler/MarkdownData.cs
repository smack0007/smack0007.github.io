using System;

namespace compiler
{
    public class MarkdownData
    {
        public FrontMatter FrontMatter { get; }

        public string Content { get; }

        public MarkdownData(FrontMatter frontMatter, string content)
        {
            this.FrontMatter = frontMatter;
            this.Content = content;
        }
    }
}
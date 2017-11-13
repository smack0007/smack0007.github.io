using System;

namespace compiler
{
    public class PostData
    {
        public FrontMatter FrontMatter { get; }

        public string Content { get; }

        public PostData(FrontMatter frontMatter, string content)
        {
            this.FrontMatter = frontMatter;
            this.Content = content;
        }
    }
}
using System;

namespace compiler
{
    public class PostData
    {
        public FrontMatter FrontMatter { get; }

        public string Content { get; }

        public string Path { get; }

        public string Title => this.FrontMatter["Title"];

        public PostData(FrontMatter frontMatter, string content, string path)
        {
            this.FrontMatter = frontMatter;
            this.Content = content;
            this.Path = path;
        }
    }
}
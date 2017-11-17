using System;
using System.Globalization;

namespace compiler
{
    public class PostData
    {
        public FrontMatter FrontMatter { get; }

        public string Content { get; }

        public string Excerpt { get; }
            
        public string Path { get; }

        public string Url => this.Path.Replace("\\", "/");

        public string Title => this.FrontMatter["Title"];

        public DateTime SortDate => DateTime.ParseExact(this.FrontMatter["Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        public string Date => this.SortDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-us"));

        public string Category => this.FrontMatter["Category"];

        public PostData(FrontMatter frontMatter, string content, string path)
        {
            this.FrontMatter = frontMatter;
            this.Content = content;
            this.Path = path;

            var index = this.Content.IndexOf("<!--more-->");

            if (index >= 0)
            {
                this.Excerpt = this.Content.Substring(0, index);
            }
            else
            {
                this.Excerpt = this.Content;
            }
        }
    }
}
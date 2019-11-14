using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace compiler
{
    public class PostData : BaseData
    {
        private Func<PostData, string> _renderPostHeader;

        public FrontMatter FrontMatter { get; }

        public string Content { get; }

        public string Excerpt { get; }

        public bool HasExcerpt { get; }
            
        public string Path { get; }

        public string Url => Path.Replace("\\", "/");

        public string Title => FrontMatter["Title"];

        public string Subtitle => FrontMatter["Subtitle"];

        public DateTime SortDate => DateTime.ParseExact(FrontMatter["Date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        public string Date => SortDate.ToString("MMMM dd, yyyy", Utility.Culture);

        public string Category => FrontMatter["Category"];

        public IEnumerable<string> Tags { get; }

        public PostData(FrontMatter frontMatter, string content, string path, Func<PostData, string> renderPostHeader)
        {
            FrontMatter = frontMatter;
            Content = content;
            Path = path;
            _renderPostHeader = renderPostHeader;

            var index = Content.IndexOf("<!--more-->");

            if (index >= 0)
            {
                Excerpt = Content.Substring(0, index);
                HasExcerpt = true;
            }
            else
            {
                Excerpt = Content;
                HasExcerpt = false;
            }

            Tags = FrontMatter["Tags"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
        }

        public string PostHeader()
        {
            return _renderPostHeader(this);
        }
    }
}
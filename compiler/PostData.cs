using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace compiler
{
    public class PostData : BaseData
    {
        private Func<PostData, string> _renderPostHeader;

        public string FileName { get; }

        public string OutputFileName { get; }

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

        public IEnumerable<string> Tags { get; }

        public PostData? OlderPost { get; set; }

        public PostData? NewerPost { get; set; }

        public PostData(
            string fileName,
            string outputFileName,
            FrontMatter frontMatter,
            string content,
            string path,
            Func<PostData, string> renderPostHeader)
        {
            FileName = fileName;
            OutputFileName = outputFileName;
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

        public string GetTagLink(string tagName)
        {
            return $"tags/{Utility.InflectFileName(tagName)}/index.html";
        }
    }
}
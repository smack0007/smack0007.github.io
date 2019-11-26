using System;
using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class IndexData
    {
        private readonly Func<PostData, string> _renderPostHeader;

        public IEnumerable<PostData> Posts { get; }

        public IndexData(IEnumerable<PostData> posts, Func<PostData, string> renderPostHeader)
        {
            this.Posts = posts;
            _renderPostHeader = renderPostHeader;
        }

        public string PostHeader(PostData post)
        {
            return _renderPostHeader(post);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace compiler
{
    public class IndexData
    {
        public IEnumerable<PostData> Posts { get; }

        public IndexData(IEnumerable<PostData> posts)
        {
            this.Posts = posts.OrderByDescending(x => x.SortDate);
        }
    }
}
using System.Collections.Generic;

namespace compiler
{
    public class TagsData : BaseData
    {
        public IEnumerable<TagData> Tags { get; }

        public TagsData(IEnumerable<TagData> tags)
        {
            Tags = tags;
        }
    }
}
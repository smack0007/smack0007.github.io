using System.Collections.Generic;

namespace compiler
{
    public class TagData : BaseData
    {
        public string Name { get; }

        public string Path { get; }

        public string Url => Path.Replace("\\", "/");

        public IReadOnlyList<PostData> Posts { get; }

        public TagData(string name, string path, IReadOnlyList<PostData> posts)
        {
            Name = name;
            Path = path;
            Posts = posts;
        }
    }
}
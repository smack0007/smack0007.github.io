using System;

namespace compiler
{
    public class SiteData
    {
        public string FileName { get; }

        public string BasePath { get; }

        public string Title { get; }

        public string Body { get; }

        public SiteData(string fileName, string basePath, string title, string body)
        {
            this.FileName = fileName;
            this.BasePath = basePath;
            this.Title = title;
            this.Body = body;
        }
    }
}
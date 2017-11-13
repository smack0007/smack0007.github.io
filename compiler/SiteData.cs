using System;

namespace compiler
{
    public class SiteData
    {
        public string BasePath { get; }

        public string Title { get; }

        public string Body { get; }

        public SiteData(string basePath, string title, string body)
        {
            this.BasePath = basePath;
            this.Title = title;
            this.Body = body;
        }
    }
}
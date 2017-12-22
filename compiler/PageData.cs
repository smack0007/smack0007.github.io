using System;

namespace compiler
{
    public class PageData
    {
        public string FileName { get; }

        public string Url => this.FileName.Replace("\\", "/").Replace(".md", ".html");

        public string BasePath { get; }

        public string Title { get; }

        public string Body { get; }

        public bool ShowPagination { get; }

        public string PaginationOlderLink { get; }

        public string PaginationNewerLink { get; }

        public PageData(string fileName, string basePath, string title, string body, bool showPagination, string paginationOlderLink, string paginationNewerLink)
        {
            this.FileName = fileName;
            this.BasePath = basePath;
            this.Title = title;
            this.Body = body;
            this.ShowPagination = showPagination;
            this.PaginationOlderLink = paginationOlderLink;
            this.PaginationNewerLink = paginationNewerLink;
        }
    }
}
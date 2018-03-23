using System;
using System.Collections.Generic;
using System.Net;

namespace compiler
{
    public delegate IEnumerable<string> Template<TData>(TData data);

    public static class TemplateExtensions
    {   
        public static string Render(this IEnumerable<string> templateResult) => string.Join(Environment.NewLine, templateResult);
                
        public static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        public static string HtmlDecode(string input) => WebUtility.HtmlDecode(input);

        public static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
        
        public static string Include<TData>(Template<TData> template, TData data) =>
            template(data).Render();
    }
}
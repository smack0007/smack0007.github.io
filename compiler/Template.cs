using System;
using System.Collections.Generic;
using System.Net;

namespace compiler
{
    public delegate IEnumerable<string> TemplateFunc<TData>(TData data);

    public abstract class Template<T>
    {
        public string Render(T data)
        {
            return string.Join(Environment.NewLine, RenderTemplate(data));
        }

        protected abstract IEnumerable<string> RenderTemplate(T data);
                
        protected static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        protected static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
        
        protected static string Include<TData>(TemplateFunc<TData> template, TData data) =>
            string.Join(Environment.NewLine, template(data));
    }

    public static class Template
    {   
        public static string Render(this IEnumerable<string> templateResult) => string.Join(Environment.NewLine, templateResult);
                
        public static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        public static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
        
        public static string Include<TData>(TemplateFunc<TData> template, TData data) =>
            template(data).Render();
    }
}
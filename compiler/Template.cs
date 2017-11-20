using System;
using System.Collections.Generic;
using System.Net;

namespace compiler
{
    public abstract class Template<T>
    {
        public string Render(T data)
        {
            return string.Join(Environment.NewLine, RenderTemplate(data));
        }

        protected abstract IEnumerable<string> RenderTemplate(T data);

        protected static string Include<TData>(Func<TData, IEnumerable<string>> renderFunc, TData data) => string.Join(Environment.NewLine, renderFunc(data));

        protected static string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
        
        protected static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);
    }
}
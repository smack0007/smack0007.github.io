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

        protected string If(bool condition, string trueString, string falseString = null) => condition ? trueString : falseString;
        
        protected string HtmlEncode(string input) => WebUtility.HtmlEncode(input);
    }
}
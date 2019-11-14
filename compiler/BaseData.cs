using System.Net;

namespace compiler
{
    public class BaseData
    {
        public static string HtmlEncode(string input) => WebUtility.HtmlEncode(input);

        public static string HtmlDecode(string input) => WebUtility.HtmlDecode(input);
    }
}

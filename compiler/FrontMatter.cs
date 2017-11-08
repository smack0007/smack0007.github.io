using System;
using System.Collections.Generic;

namespace compiler
{
    public class FrontMatter
    {
        private Dictionary<string, string> data;

        public string this[string key]
        {
            get
            {
                if (this.data.TryGetValue(key, out var value))
                {
                    return value;
                }

                return string.Empty;
            }
        }

        private FrontMatter(Dictionary<string, string> data)
        {
            this.data = data ??
                throw new ArgumentNullException(nameof(data));
        }

        public static FrontMatter Parse(string[] lines)
        {
            var data = new Dictionary<string, string>();

            string key = null;

            foreach (var frontMatterLine in lines)
            {
                string line = frontMatterLine;

                int colon = line.IndexOf(':');
                if (colon != -1)
                {
                    key = line.Substring(0, colon);
                    line = line.Substring(colon).TrimStart(':').Trim();
                }

                if (key != null)
                {
                    data[key] = line;
                }
            }

            return new FrontMatter(data);
        }
    }
}

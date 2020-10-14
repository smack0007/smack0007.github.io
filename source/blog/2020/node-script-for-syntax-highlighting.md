---
Title: Node script for syntax highlighting
Subtitle: 
Date: 2020-10-14
Tags: node, javascript, highlightjs, csharp
---

I decided to switch my blog to use highlightjs instead of the mixture of my own library and
[ColorCode](https://github.com/windows-toolkit/ColorCode-Universal). I still wanted everything to rendered statically
though so I decided to use [highlight.js](https://highlightjs.org/) as it already offered instructions on how to use the
library from node.

<!--more-->

I wanted to be able to specify what the language is being highligthed and pass the code thru stdin. Here's the script I
came up with:

```javascript
const fs = require('fs');
const hljs = require(__dirname + '/../ext/highlight.js');
const process = require('process');

const stdin = fs.readFileSync(0, 'utf-8');

console.info(hljs.highlight(process.argv[2], stdin).value);
```

I didn't feel like bothering with node_modules to the highlight.js library is just placed directly in the `ext`
directory for my blog and I reference it directly from the script. The script can be invoked from the command line
like so:

```cmd
node highlight.js html < file.html
```

Where the `html` after the `highlight.js` is the language name and `< file.hmtml` indicates to pass the contents of
`file.html` via stdin. The script can be invoked from C# like so:

```csharp
public static string Highlight(string language, string input)
{
    using var process = new Process();
    process.StartInfo.FileName = "node.exe";
    process.StartInfo.Arguments = $"highlight.js {language}";
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardInput = true;
    process.StartInfo.UseShellExecute = false;

    process.Start();

    using (var sw = process.StandardInput)
        sw.WriteLine(input);

    var output = process.StandardOutput.ReadToEnd();

    process.WaitForExit();

    return output;
}
```

---
Title: Convert TypeScript AST to JSON
Subtitle: 
Date: 2021-01-13
Tags: node, typescript
---

I needed to convert a TypeScript AST (Abstract Syntax Tree) to json so that I can consume it in
a C# application so I wrote a tiny node script to convert the AST to json.

<!--more-->

```js
// Inspiration for this script taken from StackOverflow: https://stackoverflow.com/a/20197641/26566
const fs = require('fs');
const ts = require('typescript');
const process = require('process');

const source = fs.readFileSync(process.argv[2], 'utf-8');

const sourceFile = ts.createSourceFile(process.argv[2], source, ts.ScriptTarget.Latest, true);

// Add an ID to every node in the tree to make it easier to identify in
// the consuming application.
let nextId = 0;
function addId(node) {
    nextId++;
    node.id = nextId;
    ts.forEachChild(node, addId);
}
addId(sourceFile);

// No need to save the source again.
delete sourceFile.text;

const cache = [];
const json = JSON.stringify(sourceFile, (key, value) => {
  // Discard the following.
  if (key === 'flags' || key === 'transformFlags' || key === 'modifierFlagsCache') {
      return;
  }
  
  // Replace 'kind' with the string representation.
  if (key === 'kind') {
      value = ts.SyntaxKind[value];
  }
  
  if (typeof value === 'object' && value !== null) {
    // Duplicate reference found, discard key
    if (cache.includes(value)) return;

    cache.push(value);
  }
  return value;
});

console.info(json);
```

The script will write the json to standard output. The script is meant to be invoked like so:

```cmd
node ts2json.ts input.d.ts > output.json
```

I've only tested it so far on `.d.ts` files but there shouldn't be any reason it won't work
on normal ts files.

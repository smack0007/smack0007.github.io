const fs = require('fs');
const hljs = require(__dirname + '/../ext/highlight.js');
const process = require('process');

const stdin = fs.readFileSync(0, 'utf-8');

console.info(hljs.highlight(process.argv[2], stdin).value);
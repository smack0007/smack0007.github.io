"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const shelljs_1 = require("shelljs");
const path_1 = require("path");
shelljs_1.cd(path_1.join(__dirname, "..", "source"));
const mdFileNames = [];
// function findAllFiles(directory: string, glob: string)
for (const fileName of shelljs_1.ls("**/*.md")) {
    mdFileNames.push(fileName);
}
console.info("mdFileNames", mdFileNames);

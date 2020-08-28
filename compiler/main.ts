import { cd, ls } from "shelljs";
import { join } from "path";

cd(join(__dirname, "..", "source"));

const mdFileNames: string[] = [];

// function findAllFiles(directory: string, glob: string)
for (const fileName of ls("**/*.md")) {
    mdFileNames.push(fileName);
}

console.info("mdFileNames", mdFileNames);

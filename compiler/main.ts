import { cd, ls } from "shelljs";
import { join } from "path";

cd(join(__dirname, "..", "source"));

const mdFileNames: string[] = [];

for (const fileName of ls("**/*.md")) {
    mdFileNames.push(fileName);
}

console.info("mdFileNames", mdFileNames);

import { chDir, join, listFiles } from "./node";

chDir(join(__dirname, "..", "source"));

main();

async function main() {
    const mdFileNames: string[] = [];

    for (const fileName of await listFiles(".", ".md")) {
        mdFileNames.push(fileName);
    }

    console.info("mdFileNames", mdFileNames);
}

import { chdir } from "process";
import { join, parse } from "path";
import { ensureDirectory, listFiles, readFile, writeFile } from "./utils";
import * as marked from "marked";

const ROOT_DIRECTORY = join(__dirname, "..");
const INPUT_DIRECTORY = join(ROOT_DIRECTORY, "source");
const OUTPUT_DIRECTORY = join(ROOT_DIRECTORY, "bin");

interface FrontMatter {
    title: string;
    subtitle: string;
    date: Date;
    tags: string[];
}

interface PageData {
    frontMatter: FrontMatter;
    contents: string;
}

const pages: Record<string, PageData> = {};

main();

async function main() {
    await ensureDirectory(OUTPUT_DIRECTORY);

    chdir(INPUT_DIRECTORY);

    for (const inputFileName of await listFiles(".", ".md")) {
        const page = await parsePage(await readFile(inputFileName));

        const pathParts = parse(inputFileName);
        const outputFileName = join(pathParts.dir, pathParts.name) + ".html";

        pages[inputFileName] = page;

        await ensureDirectory(join(OUTPUT_DIRECTORY, pathParts.dir));
        await writeFile(join(OUTPUT_DIRECTORY, outputFileName), page.contents);
    }
}

async function parsePage(data: string): Promise<PageData> {
    const endFrontMatter = data.indexOf("---", 1);
    const frontMatterLength = endFrontMatter + "---".length;
    const frontMatterData = data.substring(0, frontMatterLength).trim();
    const markdownData = data.substring(frontMatterLength).trim();

    const frontMatter: FrontMatter = {
        title: "",
        subtitle: "",
        date: new Date(Date.now()),
        tags: [],
    };

    for (let line of frontMatterData.split("\n")) {
        line = line.trim();

        if (line.startsWith("---")) {
            continue;
        }

        if (line.startsWith("Title: ")) {
            frontMatter.title = line.substring("Title: ".length);
        } else if (line.startsWith("Subtitle: ")) {
            frontMatter.subtitle = line.substring("Subtitle: ".length);
        } else if (line.startsWith("Date: ")) {
            frontMatter.date = new Date(
                Date.parse(line.substring("Date: ".length))
            );
        } else if (line.startsWith("Tags: ")) {
            frontMatter.tags = line
                .substring("Tags: ".length)
                .split(",")
                .map((x) => x.trim())
                .filter((x) => x.length > 0);
        }
    }

    const contents = await marked(markdownData);

    return {
        frontMatter,
        contents,
    };
}

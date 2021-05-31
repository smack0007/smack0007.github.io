import { chdir } from "process";
import { join, parse, sep as PATH_SEPERATOR } from "path";
import {
    copyDirectory,
    ensureDirectory,
    listFiles,
    readFile,
    writeFile,
} from "./utils";
import * as marked from "marked";
import * as sass from "sass";
import { FrontMatter, Page } from "./types";
import { PageTemplate, PostTemplate } from "./templates";

const ROOT_DIRECTORY = join(__dirname, "..");
const INPUT_DIRECTORY = join(ROOT_DIRECTORY, "source");
const OUTPUT_DIRECTORY = join(ROOT_DIRECTORY, "bin");

main();

async function main() {
    await ensureDirectory(OUTPUT_DIRECTORY);

    chdir(INPUT_DIRECTORY);

    //
    // Markdown
    //

    for (const inputFileName of await listFiles(".", ".md")) {
        let page = await parsePage(
            inputFileName,
            await readFile(inputFileName)
        );

        const pathParts = parse(inputFileName);
        await ensureDirectory(join(OUTPUT_DIRECTORY, pathParts.dir));

        if (inputFileName.startsWith("blog" + PATH_SEPERATOR)) {
            page = {
                frontMatter: page.frontMatter,
                url: page.url,
                contents: PostTemplate(page) as string,
            };
        }

        await writeFile(
            join(OUTPUT_DIRECTORY, page.url.replace("/", PATH_SEPERATOR)),
            PageTemplate(page) as string
        );
    }

    //
    // SCSS
    //

    var result = sass.renderSync({
        file: join(INPUT_DIRECTORY, "css", "style.scss"),
        importer: (
            url: string,
            prev: string,
            done: (data: sass.ImporterReturnType) => void
        ) => {
            console.info(url);
        },
    });

    await ensureDirectory(join(OUTPUT_DIRECTORY, "css"));
    await writeFile(
        join(OUTPUT_DIRECTORY, "css", "style.css"),
        result.css.toString()
    );

    //
    // Fonts
    //

    copyDirectory(
        join(INPUT_DIRECTORY, "fonts"),
        join(OUTPUT_DIRECTORY, "fonts")
    );
}

async function parsePage(inputFileName: string, data: string): Promise<Page> {
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

    const pathParts = parse(inputFileName);
    const outputFileName = (
        join(pathParts.dir, pathParts.name) + ".html"
    ).replace(PATH_SEPERATOR, "/");

    const contents = await marked(markdownData);

    return {
        frontMatter,
        url: outputFileName,
        contents,
    };
}

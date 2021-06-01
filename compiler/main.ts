import { chdir } from "process";
import { join, parse, sep as PATH_SEPERATOR } from "path";
import { copyDirectory, ensureDirectory, listFiles, readFile, writeFile } from "./utils";
import * as marked from "marked";
import * as sass from "sass";
import { FrontMatter, Page, Post } from "./types";
import { PageTemplate, PostTemplate } from "./templates";
import { IndexTemplate } from "~/source/templates";

const ROOT_DIRECTORY = join(__dirname, "..");
const INPUT_DIRECTORY = join(ROOT_DIRECTORY, "source");
const OUTPUT_DIRECTORY = join(ROOT_DIRECTORY, "bin");

interface MarkdownFilesResult {
    posts: Post[];
    pages: Page[];
}

interface MarkdownResult {
    frontMatter: FrontMatter;
    contents: string;
}

main();

async function main() {
    await ensureDirectory(OUTPUT_DIRECTORY);

    chdir(INPUT_DIRECTORY);

    const { posts, pages } = await parseMarkdownFiles();

    posts.sort((a, b) => b.frontMatter.date.getTime() - a.frontMatter.date.getTime());

    await writeIndexFiles(posts);

    await compileScss();
    await copyFonts();
}

async function parseMarkdownFiles(): Promise<MarkdownFilesResult> {
    const posts: Post[] = [];
    const pages: Page[] = [];

    for (const inputFileName of await listFiles(".", ".md")) {
        const pathParts = parse(inputFileName);
        await ensureDirectory(join(OUTPUT_DIRECTORY, pathParts.dir));

        const markdownResult = await parseMarkdown(await readFile(inputFileName));

        const url = (join(pathParts.dir, pathParts.name) + ".html").replace(PATH_SEPERATOR, "/");

        let output = "";

        if (inputFileName.startsWith("blog" + PATH_SEPERATOR)) {
            const post = createPost(markdownResult, url);

            posts.push(post);

            output = PostTemplate(post);
            output = PageTemplate(createPage({ frontMatter: post.frontMatter, contents: output }, url));
        } else {
            const page = createPage(markdownResult, url);

            pages.push(page);

            output = PageTemplate(page) as string;
        }

        await writeFile(join(OUTPUT_DIRECTORY, url.replace("/", PATH_SEPERATOR)), output);
    }

    return {
        posts,
        pages,
    };
}

function createFrontMatter(data?: Partial<FrontMatter>): FrontMatter {
    return {
        title: data?.title || "",
        subtitle: data?.subtitle || "",
        date: data?.date || new Date(Date.now()),
        tags: data?.tags || [],
    };
}

async function parseMarkdown(data: string): Promise<MarkdownResult> {
    const endFrontMatter = data.indexOf("---", 1);
    const frontMatterLength = endFrontMatter + "---".length;
    const frontMatterData = data.substring(0, frontMatterLength).trim();
    const markdownData = data.substring(frontMatterLength).trim();

    const frontMatter: FrontMatter = createFrontMatter();

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
            frontMatter.date = new Date(Date.parse(line.substring("Date: ".length)));
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

function createPost(data: MarkdownResult, url: string): Post {
    const excerptIndex = data.contents.indexOf("<!--more-->");

    const hasExcerpt = excerptIndex !== -1;
    let excerpt = hasExcerpt ? data.contents.substring(0, excerptIndex).trim() : data.contents;

    return {
        frontMatter: data.frontMatter,
        contents: data.contents,
        url,
        hasExcerpt,
        excerpt,
    };
}

function createPage(data: MarkdownResult, url: string): Page {
    return {
        frontMatter: data.frontMatter,
        contents: data.contents,
        url,
    };
}

async function writeIndexFiles(posts: Post[]): Promise<void> {
    let output = IndexTemplate(posts);

    output = PageTemplate(
        createPage({ frontMatter: createFrontMatter({ title: "Index" }), contents: output }, "index.html")
    );

    await writeFile(join(OUTPUT_DIRECTORY, "index.html"), output);
}

async function compileScss(): Promise<void> {
    var result = sass.renderSync({
        file: join(INPUT_DIRECTORY, "css", "style.scss"),
        importer: (url: string, prev: string, done: (data: sass.ImporterReturnType) => void) => {
            console.info(url);
        },
    });

    await ensureDirectory(join(OUTPUT_DIRECTORY, "css"));
    await writeFile(join(OUTPUT_DIRECTORY, "css", "style.css"), result.css.toString());
}

async function copyFonts(): Promise<void> {
    await copyDirectory(join(INPUT_DIRECTORY, "fonts"), join(OUTPUT_DIRECTORY, "fonts"));
}

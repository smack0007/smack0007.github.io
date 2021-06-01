import { copyFile } from "fs/promises";
import { chdir } from "process";
import { join, parse, sep as PATH_SEPERATOR } from "path";
import * as marked from "marked";
import * as sass from "sass";
const hljs = require("highlight.js");

import { copyDirectory, ensureDirectory, listFiles, readFile, writeFile } from "./utils";
import { FrontMatter, Page, Post, Tag } from "./types";
import { PageTemplate, PostTemplate, IndexTemplate } from "./templates";
import { BLOG_TITLE, ENVIRONMENT, POSTS_PER_PAGE } from "./confg";
import { getTagUrl, inflectFileName } from "./templateUtils";
import { TagsTemplate } from "~/source/templates/tags";

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
    console.info(`Environment: ${ENVIRONMENT}`);

    marked.setOptions({
        langPrefix: "hljs ",
        highlight: function (code: string, lang: string) {
            if (!lang) {
                return;
            }

            return hljs.highlight(code, { language: lang }).value;
        },
    });

    await ensureDirectory(OUTPUT_DIRECTORY);

    chdir(INPUT_DIRECTORY);

    const { posts, pages } = await parseMarkdownFiles();

    posts.sort((a, b) => b.frontMatter.date.getTime() - a.frontMatter.date.getTime());

    await writeIndexPages(posts);
    await writeTagPages(posts);

    await compileScss();
    await copyFonts();
    await copyStaticFiles();
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

function createPage(
    data: MarkdownResult,
    url: string,
    showPagination?: boolean,
    paginationOlderLink?: string,
    paginationNewerLink?: string
): Page {
    return {
        frontMatter: data.frontMatter,
        contents: data.contents,
        url,
        showPagination,
        paginationOlderLink,
        paginationNewerLink,
    };
}

async function writeIndexPages(posts: Post[]): Promise<void> {
    await ensureDirectory(join(OUTPUT_DIRECTORY, "blog"));

    const pageCount = Math.ceil(posts.length / POSTS_PER_PAGE);

    console.info(`Index: ${posts.length} Posts / ${pageCount} Pages`);

    const getUrlForIndex = (index: number) => {
        if (index < 0 || index >= pageCount) {
            return undefined;
        }

        return index == 0 ? "index.html" : ["blog", "page" + (index + 1) + ".html"].join("/");
    };

    for (let i = 0; i < pageCount; i++) {
        const postsForPage = posts.slice(i * POSTS_PER_PAGE, i * POSTS_PER_PAGE + POSTS_PER_PAGE);

        let output = IndexTemplate(postsForPage);
        let url = getUrlForIndex(i) as string;

        output = PageTemplate(
            createPage(
                { frontMatter: createFrontMatter({ title: BLOG_TITLE }), contents: output },
                url,
                true,
                getUrlForIndex(i + 1),
                getUrlForIndex(i - 1)
            )
        );

        await writeFile(join(OUTPUT_DIRECTORY, url.replace("/", PATH_SEPERATOR)), output);
    }
}

async function writeTagPages(posts: Post[]): Promise<void> {
    await ensureDirectory(join(OUTPUT_DIRECTORY, "tags"));

    const tagMap: Record<string, Tag> = {};

    for (const post of posts) {
        for (const tag of post.frontMatter.tags) {
            if (tagMap[tag] === undefined) {
                tagMap[tag] = {
                    name: tag,
                    url: getTagUrl(tag),
                    posts: [],
                };
            }

            tagMap[tag]?.posts.push(post);
        }
    }

    const tags = Object.values(tagMap);
    tags.sort((a, b) => a.name.localeCompare(b.name));

    let output = TagsTemplate(tags);

    const url = "tags/index.html";

    output = PageTemplate(createPage({ frontMatter: createFrontMatter({ title: BLOG_TITLE }), contents: output }, url));

    await writeFile(join(OUTPUT_DIRECTORY, url.replace("/", PATH_SEPERATOR)), output);

    for (const tag of tags) {
        const pathParts = parse(tag.url.replace("/", PATH_SEPERATOR));
        await ensureDirectory(join(OUTPUT_DIRECTORY, pathParts.dir));

        const pageCount = Math.ceil(tag.posts.length / POSTS_PER_PAGE);

        console.info(`Tag: ${tag.name} ${tag.posts.length} Posts / ${pageCount} Pages`);

        const slug = inflectFileName(tag.name);

        const getUrlForIndex = (index: number) => {
            if (index < 0 || index >= pageCount) {
                return undefined;
            }

            return index == 0 ? `tags/${slug}/index.html` : `tags/${slug}/page${index + 1}.html`;
        };

        for (let i = 0; i < pageCount; i++) {
            const postsForPage = tag.posts.slice(i * POSTS_PER_PAGE, i * POSTS_PER_PAGE + POSTS_PER_PAGE);

            let output = IndexTemplate(postsForPage);
            let url = getUrlForIndex(i) as string;

            output = PageTemplate(
                createPage(
                    { frontMatter: createFrontMatter({ title: tag.name }), contents: output },
                    url,
                    true,
                    getUrlForIndex(i + 1),
                    getUrlForIndex(i - 1)
                )
            );

            await writeFile(join(OUTPUT_DIRECTORY, url.replace("/", PATH_SEPERATOR)), output);
        }
    }
}

async function compileScss(): Promise<void> {
    var result = sass.renderSync({
        file: join(INPUT_DIRECTORY, "css", "style.scss"),
        outputStyle: "compressed",
    });

    await ensureDirectory(join(OUTPUT_DIRECTORY, "css"));
    await writeFile(join(OUTPUT_DIRECTORY, "css", "style.css"), result.css.toString());
}

async function copyFonts(): Promise<void> {
    await copyDirectory(join(INPUT_DIRECTORY, "fonts"), join(OUTPUT_DIRECTORY, "fonts"));
}

async function copyStaticFiles(): Promise<void> {
    for (const file of await listFiles(INPUT_DIRECTORY, undefined, false)) {
        if (file.endsWith(".md")) {
            continue;
        }

        const pathParts = parse(file);
        copyFile(file, join(OUTPUT_DIRECTORY, pathParts.base));
    }
}

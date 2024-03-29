import { ensureDir } from "std/fs/mod.ts";
import {
  dirname,
  fromFileUrl,
  join,
  parse,
  sep as PATH_SEPERATOR,
} from "std/path/mod.ts";
import { marked } from "npm:marked";
import hljs from "npm:highlight.js";

import {
  copyDirectory,
  listFiles,
  replaceAll,
  runCommand,
  writeFile,
} from "./utils.ts";
import { FrontMatter, Page, Post, Tag } from "./types.ts";
import { PageTemplate, PostTemplate, IndexTemplate } from "./templates.ts";
import { BASE_URL, BLOG_TITLE, ENVIRONMENT, POSTS_PER_PAGE } from "./confg.ts";
import { getTagUrl, inflectFileName } from "./templateUtils.ts";
import { TagsTemplate } from "~/src/templates/tags.tsx";

const __dirname = dirname(fromFileUrl(import.meta.url));
const IS_WINDOWS = Deno.build.os === "windows";

const ROOT_DIRECTORY = join(__dirname, "..");
const INPUT_DIRECTORY = join(ROOT_DIRECTORY, "src");
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

async function main(): Promise<void> {
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

  await ensureDir(OUTPUT_DIRECTORY);

  Deno.chdir(INPUT_DIRECTORY);

  const { posts, pages } = await parseMarkdownFiles();

  posts.sort(
    (a, b) => b.frontMatter.date.getTime() - a.frontMatter.date.getTime()
  );

  await writeIndexPages(posts);
  await writeTagPages(posts);

  await compileScss();
  await copyFonts();
  await copyStaticFiles();

  await writeRssFeed(posts);
  await writeSiteMap(pages);
}

async function parseMarkdownFiles(): Promise<MarkdownFilesResult> {
  const posts: Post[] = [];
  const pages: Page[] = [];

  for (const inputFileName of await listFiles(".", ".md")) {
    const pathParts = parse(inputFileName);
    await ensureDir(join(OUTPUT_DIRECTORY, pathParts.dir));

    const markdownResult = await parseMarkdown(
      await Deno.readTextFile(inputFileName)
    );

    const url = replaceAll(
      join(pathParts.dir, pathParts.name) + ".html",
      PATH_SEPERATOR,
      "/"
    );

    let output = "";

    if (inputFileName.startsWith("blog" + PATH_SEPERATOR)) {
      const post = createPost(markdownResult, url);
      posts.push(post);

      output = PostTemplate(post);

      const page = createPage(
        { frontMatter: post.frontMatter, contents: output },
        url
      );
      pages.push(page);

      output = PageTemplate(page);
    } else {
      const page = createPage(markdownResult, url);

      pages.push(page);

      output = PageTemplate(page) as string;
    }

    await writeFile(
      join(OUTPUT_DIRECTORY, replaceAll(url, "/", PATH_SEPERATOR)),
      output
    );
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
  const excerpt = hasExcerpt
    ? data.contents.substring(0, excerptIndex).trim()
    : data.contents;

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
  await ensureDir(join(OUTPUT_DIRECTORY, "blog"));

  const pageCount = Math.ceil(posts.length / POSTS_PER_PAGE);

  console.info(`Index: ${posts.length} Posts / ${pageCount} Pages`);

  const getUrlForIndex = (index: number) => {
    if (index < 0 || index >= pageCount) {
      return undefined;
    }

    return index == 0
      ? "index.html"
      : ["blog", "page" + (index + 1) + ".html"].join("/");
  };

  for (let i = 0; i < pageCount; i++) {
    const postsForPage = posts.slice(
      i * POSTS_PER_PAGE,
      i * POSTS_PER_PAGE + POSTS_PER_PAGE
    );

    let output = IndexTemplate(postsForPage);
    const url = getUrlForIndex(i) as string;

    output = PageTemplate(
      createPage(
        {
          frontMatter: createFrontMatter({ title: BLOG_TITLE }),
          contents: output,
        },
        url,
        true,
        getUrlForIndex(i + 1),
        getUrlForIndex(i - 1)
      )
    );

    await writeFile(
      join(OUTPUT_DIRECTORY, replaceAll(url, "/", PATH_SEPERATOR)),
      output
    );
  }
}

async function writeTagPages(posts: Post[]): Promise<void> {
  await ensureDir(join(OUTPUT_DIRECTORY, "tags"));

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

  output = PageTemplate(
    createPage(
      {
        frontMatter: createFrontMatter({ title: BLOG_TITLE }),
        contents: output,
      },
      url
    )
  );

  await writeFile(
    join(OUTPUT_DIRECTORY, replaceAll(url, "/", PATH_SEPERATOR)),
    output
  );

  for (const tag of tags) {
    const pathParts = parse(replaceAll(tag.url, "/", PATH_SEPERATOR));
    await ensureDir(join(OUTPUT_DIRECTORY, pathParts.dir));

    const pageCount = Math.ceil(tag.posts.length / POSTS_PER_PAGE);

    console.info(
      `Tag: ${tag.name} ${tag.posts.length} Posts / ${pageCount} Pages`
    );

    const slug = inflectFileName(tag.name);

    const getUrlForIndex = (index: number) => {
      if (index < 0 || index >= pageCount) {
        return undefined;
      }

      return index == 0
        ? `tags/${slug}/index.html`
        : `tags/${slug}/page${index + 1}.html`;
    };

    for (let i = 0; i < pageCount; i++) {
      const postsForPage = tag.posts.slice(
        i * POSTS_PER_PAGE,
        i * POSTS_PER_PAGE + POSTS_PER_PAGE
      );

      let output = IndexTemplate(postsForPage);
      const url = getUrlForIndex(i) as string;

      output = PageTemplate(
        createPage(
          {
            frontMatter: createFrontMatter({ title: tag.name }),
            contents: output,
          },
          url,
          true,
          getUrlForIndex(i + 1),
          getUrlForIndex(i - 1)
        )
      );

      await writeFile(
        join(OUTPUT_DIRECTORY, replaceAll(url, "/", PATH_SEPERATOR)),
        output
      );
    }
  }
}

async function compileScss(): Promise<void> {
  // TODO: Reactivate this when node-sass works with deno
  //   const result = sass.renderSync({
  //     file: join(INPUT_DIRECTORY, "css", "style.scss"),
  //     outputStyle: "compressed",
  //   });

  const [nodeSassResult, nodeSassOutput] = await runCommand(
    ROOT_DIRECTORY,
    IS_WINDOWS ? "npx.cmd" : "npx",
    "-y",
    "node-sass",
    join(INPUT_DIRECTORY, "css", "style.scss"),
    "--output-style=compressed"
  );

  if (nodeSassResult !== 0) {
    console.error(`node-sass failed with error code ${nodeSassResult}.`);
    return;
  }

  await ensureDir(join(OUTPUT_DIRECTORY, "css"));
  await writeFile(join(OUTPUT_DIRECTORY, "css", "style.css"), nodeSassOutput);

  console.info("Calling PurgeCSS...");
  const [purgeResult] = await runCommand(
    OUTPUT_DIRECTORY,
    IS_WINDOWS ? "npx.cmd" : "npx",
    "-y",
    "purgecss",
    "--css",
    "./css/style.css",
    "--content",
    "./**/*.html",
    "--output",
    "./css/style.css"
  );

  if (purgeResult !== 0) {
    console.error(`PurgeCSS failed with error code ${purgeResult}.`);
  }
}

async function copyFonts(): Promise<void> {
  await copyDirectory(
    join(INPUT_DIRECTORY, "fonts"),
    join(OUTPUT_DIRECTORY, "fonts")
  );
}

async function copyStaticFiles(): Promise<void> {
  for (const file of await listFiles(INPUT_DIRECTORY, undefined, false)) {
    if (file.endsWith(".md")) {
      continue;
    }

    const pathParts = parse(file);
    await Deno.copyFile(file, join(OUTPUT_DIRECTORY, pathParts.base));
  }
}

async function writeRssFeed(posts: Post[]): Promise<void> {
  let output = "";

  if (posts[0] === undefined) {
    return;
  }

  const pubDate = posts[0].frontMatter.date.toUTCString();

  output += '<?xml version="1.0" encoding="UTF-8"?>\n';
  output += '<rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom">\n';
  output += "\t<channel>\n";
  output += `\t<atom:link href="${BASE_URL}/feed.rss" rel="self" type="application/rss+xml" />\n`;
  output += `\t<title>${BLOG_TITLE}</title>\n`;
  output += `\t<description>${BLOG_TITLE}</description>\n`;
  output += `\t<link>${BASE_URL}/</link>\n`;
  output += `\t<lastBuildDate>${pubDate}</lastBuildDate>\n`;
  output += `\t<pubDate>${pubDate}</pubDate>\n`;
  output += "\t<ttl>1800</ttl>\n";

  for (const post of posts.slice(0, 20)) {
    output += "\t<item>\n";
    output += `\t\t<title><![CDATA[${post.frontMatter.title}]]></title>\n`;
    output += `\t\t<description><![CDATA[${post.excerpt}]]></description>\n`;
    output += `\t\t<link>${BASE_URL}/${post.url}</link>\n`;
    output += `\t\t<guid>${BASE_URL}/${post.url}</guid>\n`;
    output += `\t\t<pubDate>${post.frontMatter.date.toUTCString()}</pubDate>\n`;
    output += "\t</item>\n";
  }

  output += "\t</channel>\n";
  output += "</rss>\n";
  output += "\n";

  await writeFile(join(OUTPUT_DIRECTORY, "feed.rss"), output);
}

async function writeSiteMap(pages: Page[]): Promise<void> {
  let output = "";

  output += '<?xml version="1.0" encoding="UTF-8" ?>\n';
  output += '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n';
  output += "\t<url>\n";
  output += `\t\t<loc>${BASE_URL}/index.html</loc>\n`;
  output += "\t</url>\n";

  for (const page of pages) {
    output += "\t<url>\n";
    output += `\t\t<loc>${BASE_URL}/${page.url}</loc>\n`;
    output += "\t</url>\n";
  }

  output += "</urlset>\n";
  output += "\n";

  await writeFile(join(OUTPUT_DIRECTORY, "sitemap.xml"), output);
}

import * as process from "process";

export const ENVIRONMENT = process.argv[2] ?? "localhost";
export const BLOG_TITLE = "The Blog of Zachary Snow";
export const BASE_URL = ENVIRONMENT === "CI" ? "https://smack0007.github.io" : "/";
export const POSTS_PER_PAGE = 5;

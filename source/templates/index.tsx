import { html, Post } from "~/compiler/index.ts";
import { PostHeaderTemplate } from "./postHeader.tsx";

export function IndexTemplate(posts: Post[]): string {
  return posts
    .map((post) => (
      <div class="post">
        {PostHeaderTemplate(post)}
        <div class="content">
          {post.excerpt}

          {post.hasExcerpt && (
            <a class="readMore" href={post.url}>
              Read More
            </a>
          )}
        </div>
      </div>
    ))
    .join("");
}

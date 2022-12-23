import { html, Post } from "~/compiler/index.ts";
import { PostHeaderTemplate } from "./postHeader.tsx";

export function PostTemplate(post: Post): string {
  return (
    <div class="post">
      {PostHeaderTemplate(post)}

      <div class="content">{post.contents}</div>
    </div>
  );
}

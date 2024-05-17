import { html, Post } from "~/compiler/index.ts";
import { PostHeaderTemplate } from "./postHeader.tsx";

export const PostTemplate = (post: Post) => (
  <div class="post">
    {PostHeaderTemplate(post)}

    <div class="content">{post.contents}</div>
  </div>
);

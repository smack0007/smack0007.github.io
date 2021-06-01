import { html, Post } from "~/compiler";
import { PostHeaderTemplate } from "./postHeader";

export function PostTemplate(post: Post): string {
    return (
        <div class="post">
            {PostHeaderTemplate(post)}

            <div class="content">{post.contents}</div>
        </div>
    );
}

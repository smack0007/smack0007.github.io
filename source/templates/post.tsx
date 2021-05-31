import { html, Page } from "~/compiler";
import { PostHeaderTemplate } from "./postHeader";

export function PostTemplate(post: Page) {
    return (
        <div class="post">
            {PostHeaderTemplate(post)}

            <div class="content">{post.contents}</div>
        </div>
    );
}

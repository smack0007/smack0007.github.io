import { html, Post } from "~/compiler";
import { PostHeaderTemplate } from "./postHeader";

export function IndexTemplate(posts: Post[]): string {
    return posts
        .map((post) => (
            <div class="post">
                {PostHeaderTemplate(post)}
                <div class="content">
                    {post.excerpt}

                    {post.hasExcerpt && (
                        <a class="readMore" href="@(post.Url)">
                            Read More
                        </a>
                    )}
                </div>
            </div>
        ))
        .join("");
}

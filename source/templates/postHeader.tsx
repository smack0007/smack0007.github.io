import { encode, formatDate, html, Post } from "~/compiler";

export function PostHeaderTemplate(post: Post): string {
    return (
        <div class="post-header">
            <h2>
                <a href="@Url">{encode(post.frontMatter.title)}</a>
            </h2>
            {post.frontMatter.subtitle.length > 0 && <h3>{encode(post.frontMatter.subtitle)}</h3>}
            <div class="meta">
                <span class="date">
                    <span class="icon-calendar"></span>
                    {formatDate(post.frontMatter.date)}
                </span>
                <span class="tags">
                    {post.frontMatter.tags.map((tag) => (
                        <a href="">
                            <span class="icon icon-price-tags"></span>
                            <span class="tagName">{encode(tag)}</span>
                        </a>
                    ))}
                </span>
            </div>
        </div>
    );
}

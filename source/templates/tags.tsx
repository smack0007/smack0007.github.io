import { html, Tag } from "~/compiler";

export function TagsTemplate(tags: Tag[]): string {
    return (
        <ul>
            {tags.forEach((tag) => {
                <li class="tag">
                    <a href={tag.url}>
                        {tag.name} ({tag.posts.length})
                    </a>
                </li>;
            })}
        </ul>
    );
}

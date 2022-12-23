import { html, Tag } from "~/compiler/index.ts";

export function TagsTemplate(tags: Tag[]): string {
  return (
    <ul>
      {tags.map((tag) => (
        <li class="tag">
          <a href={tag.url}>
            {tag.name} ({tag.posts.length})
          </a>
        </li>
      ))}
    </ul>
  );
}

export interface FrontMatter {
    title: string;
    subtitle: string;
    date: Date;
    tags: string[];
}

export interface Post {
    frontMatter: FrontMatter;
    url: string;
    contents: string;
    hasExcerpt: boolean;
    excerpt: string;
}

export interface Page {
    frontMatter: FrontMatter;
    url: string;
    contents: string;
}

export interface Tag {
    name: string;
    url: string;
    posts: Post[];
}

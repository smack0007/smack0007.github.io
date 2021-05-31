export interface FrontMatter {
    title: string;
    subtitle: string;
    date: Date;
    tags: string[];
}

export interface Page {
    frontMatter: FrontMatter;
    url: string;
    contents: string;
}
